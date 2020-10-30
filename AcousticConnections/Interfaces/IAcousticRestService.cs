using AcousticConnections.Enums;

namespace AcousticConnections.Interfaces
{
    public interface IAcousticRestService
    {
        /// <summary>
        /// Posts XML request to silverpop site and returns XML response as string
        /// </summary>
        /// <param name="postData">XML request</param>
        /// <param name="silverpopApiCommand"></param>
        /// <returns>XML response</returns>
        string PostAndReturnResponse(string postData, ApiCommand apiCommand, string requestUrl,
            string requestMethodType = "POST");
    }
}