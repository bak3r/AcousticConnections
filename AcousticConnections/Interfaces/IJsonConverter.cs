using AcousticConnections.DTOs;

namespace AcousticConnections.Interfaces
{
    public interface IJsonConverter
    {
        AccessTokenReply DeserializeObject<T>(string unserializedString);
    }
}