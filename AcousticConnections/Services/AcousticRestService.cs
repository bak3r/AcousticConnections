using System;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Text;
using AcousticConnections.DTOs;
using AcousticConnections.Enums;
using AcousticConnections.Interfaces;

namespace AcousticConnections.Services
{
    public class AcousticRestService : IAcousticRestService
    {
        
        private readonly IAcousticConfiguration _configuration;
        private readonly IAcousticLogger _logger;
        private readonly IAcousticStorage _repository;
        private readonly IJsonConverter _jsonConverter;

        private readonly int _organizationId;
        private readonly int _applicationId;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _refreshToken;
        private bool _loginSuccessful;
        private readonly AccessToken _accessToken;
        private readonly string _requestUrl;

        public AcousticRestService(IAcousticConfiguration configuration, IAcousticLogger logger, IAcousticStorage repository, IJsonConverter jsonConverter)
        {
            _configuration = configuration;
            _logger = logger;
            _repository = repository;
            _jsonConverter = jsonConverter;

            _organizationId = _configuration.OrganizationId;
            _applicationId = _configuration.ApplicationId;

            _clientId = _configuration.ClientId;
            _clientSecret = _configuration.ClientSecret;
            _refreshToken = _configuration.RefreshToken;

            _loginSuccessful = false;
            _requestUrl = _configuration.RequestUrl;

            _accessToken = _repository.GetAccessToken(_organizationId, _applicationId) ?? new AccessToken();
            if (_accessToken.ExpirationDate != null)
                if (_accessToken.ExpirationDate.Value > DateTime.Now)
                    _loginSuccessful = true;
        }

        private void Login()
        {
            try
            {
                var webRequest = (HttpWebRequest)WebRequest.Create("https://api-campaign-us-4.goacoustic.com/oauth/token");
                var postData = string.Format(
                    "grant_type=refresh_token&client_id={0}&client_secret={1}&refresh_token={2}",
                    _clientId, _clientSecret, _refreshToken);

                var encoding = new ASCIIEncoding();
                var data = encoding.GetBytes(postData);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = data.Length;

                using (var stream = webRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
                var result = new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd();
                var accessTokenReply = _jsonConverter.DeserializeObject<AccessTokenReply>(result);

                _accessToken.ExpirationDate = DateTime.Now.AddSeconds(accessTokenReply.expires_in);
                _accessToken.Value = accessTokenReply.access_token;

                _repository.StoreAccessToken(_accessToken.ExpirationDate, accessTokenReply.expires_in,
                    _accessToken.Value, _organizationId);

                _loginSuccessful = true;
            }
            catch (Exception e)
            {
                _logger.Error("Error occurred when retrieving/parsing access token. Message: " + e.Message);
                throw;
            }
        }

        /// <summary>
        /// Posts XML request to Acoustic REST service and returns XML response as string
        /// </summary>
        /// <returns>XML response</returns>
        public string PostAndReturnResponse(string postData, ApiCommand apiCommand, string requestUrl = null,
            string requestMethodType = "POST")
        {
            var responseFromServer = string.Empty;

            try
            {
                if (!_loginSuccessful)
                    Login();

                if (!_accessToken.ExpirationDate.HasValue)
                    Login();
                else if (_accessToken.ExpirationDate.Value.Subtract(DateTime.Now) < new TimeSpan(0, 1, 0))
                    Login();

                if (_loginSuccessful)
                {
                    if (string.IsNullOrEmpty(requestUrl))
                    {
                        requestUrl = _requestUrl;
                    }

                    // Create a request using a URL that can receive a post. 
                    var request = (HttpWebRequest)WebRequest.Create(requestUrl);
                    request.Timeout = 180000;
                    request.Headers.Add("Authorization", "Bearer " + _accessToken.Value);
                    // Set the Method property of the request to POST/GET.
                    request.Method = requestMethodType;
                    request.AllowAutoRedirect = false;


                    switch (requestMethodType)
                    {
                        case "POST":
                            if (apiCommand == ApiCommand.GdprErasure)
                            {
                                request.ContentType = "text/csv";
                            }
                            else
                            {
                                request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
                            }
                            // Create POST data and convert it to a byte array.
                            var byteArray = Encoding.UTF8.GetBytes(postData);
                            // Set the ContentLength property of the WebRequest.
                            request.ContentLength = byteArray.Length;
                            // Get the request stream.
                            var postRequestStream = request.GetRequestStream();
                            // Write the data to the request stream.
                            postRequestStream.Write(byteArray, 0, byteArray.Length);
                            // Close the Stream object.
                            postRequestStream?.Close();
                            // Get the response.
                            var postResponse = request.GetResponse();
                            // Get the stream containing content returned by the server.
                            var postResponseStream = postResponse.GetResponseStream();
                            // Open the stream using a StreamReader for easy access.
                            var reader = new StreamReader(postResponseStream);
                            // Read the content.
                            responseFromServer = reader.ReadToEnd();
                            reader.Close();
                            postResponseStream?.Close();
                            postResponse?.Close();
                            break;
                        case "GET":
                            //request.ContentType = "text/csv";
                            // Get the response.
                            var getResponse = request.GetResponse();
                            // Get the request stream.
                            var getResponseStream = getResponse.GetResponseStream();
                            // Open the stream using a StreamReader for easy access.
                            var getResponseReader = new StreamReader(getResponseStream);
                            // Read the content.
                            responseFromServer = getResponseReader.ReadToEnd();
                            // Clean up the streams.
                            getResponseReader?.Close();
                            getResponseStream?.Close();
                            getResponse?.Close();
                            break;
                    }

                    // STORE REQUEST in DB
                    _repository.InsertApiCallLog(string.Empty, postData, responseFromServer,
                        apiCommand, _applicationId);
                }
                else
                {
                    _logger.Fatal("Could not sucessfully login to silverpop with clientId: " + _clientId +
                                  " ClientSecret: " + _clientSecret + " RefreshToken: " + _refreshToken +
                                  " AccessToken: " + _accessToken.Value);
                    throw new AuthenticationException("Could not successfully login to Silverpop.");
                }
            }
            catch (WebException we)
            {
                _logger.Fatal("WebException was thrown in Silverpop.PostAndReturnResponse, ApiCommand was: " +
                              apiCommand + ". ExceptionMessage: " + we.Message + ". LoginSuccessfull: " +
                              _loginSuccessful +
                              ". AccessToken: \"" + _accessToken.Value +
                              "\". PostData: " + postData, we);
            }
            catch (Exception e)
            {
                _logger.Fatal("Exception was thrown in Silverpop.PostAndReturnResponse, ApiCommand was: " +
                              apiCommand + ". ExceptionMessage: " + e.Message + ". LoginSuccessfull: " +
                              _loginSuccessful +
                              ". AccessToken: \"" + _accessToken.Value +
                              "\". PostData: " + postData, e);
            }

            return responseFromServer;
        }
    }
}
