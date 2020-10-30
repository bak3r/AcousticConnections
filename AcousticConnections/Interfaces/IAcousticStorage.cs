using System;
using AcousticConnections.DTOs;
using AcousticConnections.Enums;

namespace AcousticConnections.Interfaces
{
    public interface IAcousticStorage
    {
        void InsertApiCallLog(string sessionId, string postData, string responseFromServer, ApiCommand apiCommand,
            int applicationId);

        void StoreAccessToken(DateTime? accessTokenExpirationDate, int expiresIn, string accessTokenValue,
            int organizationId);

        AccessToken GetAccessToken(int organizationId, int applicationId);
    }
}