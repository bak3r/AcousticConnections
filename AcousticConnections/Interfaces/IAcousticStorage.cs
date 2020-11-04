using System;
using AcousticConnections.DTOs;
using AcousticConnections.Enums;

namespace AcousticConnections.Interfaces
{
    public interface IAcousticStorage
    {
        /// <summary>
        /// Inserts API call and response to the log (database, filesystem....)
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="postData"></param>
        /// <param name="responseFromServer"></param>
        /// <param name="apiCommand"></param>
        /// <param name="applicationId"></param>
        void InsertApiCallLog(string sessionId, string postData, string responseFromServer, ApiCommand apiCommand,
            int applicationId);

        /// <summary>
        /// Stores access token to persistent storage so it can be retrieved later
        /// </summary>
        /// <param name="accessTokenExpirationDate"></param>
        /// <param name="expiresIn"></param>
        /// <param name="accessTokenValue"></param>
        /// <param name="organizationId"></param>
        void StoreAccessToken(DateTime? accessTokenExpirationDate, int expiresIn, string accessTokenValue,
            int organizationId);

        /// <summary>
        /// Retrieves access token from persistent storage
        /// </summary>
        /// <param name="organizationId"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        AccessToken GetAccessToken(int organizationId, int applicationId);
    }
}