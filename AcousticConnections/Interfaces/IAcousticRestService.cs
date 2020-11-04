using AcousticConnections.Enums;

namespace AcousticConnections.Interfaces
{
    public interface IAcousticRestService
    {
        /// <summary>
        /// Posts XML request to acoustic site and returns XML response as string
        /// </summary>
        /// <param name="postData">XML request</param>
        /// <param name="apiCommand">ApiCommand in the request - for logging purposes</param>
        /// <param name="requestUrl">REST service request url if you want to change it from the configuration default.</param>
        /// <param name="requestMethodType">POST / GET => defaults to POST</param>
        /// <returns>XML response</returns>
        string PostAndReturnResponse(string postData, ApiCommand apiCommand, string requestUrl = null,
            string requestMethodType = "POST");
    }
}