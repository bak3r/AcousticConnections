using System;

namespace AcousticConnections.Interfaces
{
    public interface IAcousticConfiguration
    {
        /// <summary>
        /// We use multiple organizations so this might not be relevant to you. It is used
        /// for logging and retrieving/storing access token from/to storage.
        /// </summary>
        int OrganizationId { get; }

        /// <summary>
        /// We have multiple applications that use same logging backend and same storage
        /// for job queueing so this might not be relevant to you. It is also used for
        /// retrieving/storing access token from/to storage.
        /// Choose any integer you want
        /// </summary>
        int ApplicationId { get; }

        /// <summary>
        /// If you're on pod 4, set this to:
        /// https://api-campaign-us-4.goacoustic.com/XMLAPI
        /// </summary>
        string RequestUrl { get; }

        /// <summary>
        /// If you're on pod 4, set this to:
        /// https://api-campaign-us-4.goacoustic.com/oauth/token
        /// </summary>
        string LoginRequestUrl { get; }

        /// <summary>
        /// This is the ClientId you get when you setup Application account access in Acoustic
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// This is the ClientSecret you get when you setup Application account access in Acoustic
        /// </summary>
        string ClientSecret { get; }

        /// <summary>
        /// This is the RefreshToken you get when you setup Application account access in Acoustic
        /// </summary>
        string RefreshToken { get; }
    }
}