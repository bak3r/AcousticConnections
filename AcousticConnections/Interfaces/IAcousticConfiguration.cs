namespace AcousticConnections.Interfaces
{
    public interface IAcousticConfiguration
    {
        int OrganizationId { get; }
        int ApplicationId { get; }
        string RequestUrl { get; }
        string ClientId { get; }
        string ClientSecret { get; }
        string RefreshToken { get; }
    }
}