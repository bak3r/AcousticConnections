using System;

namespace AcousticConnections.DTOs
{
    public class AccessToken
    {
        public DateTime? ExpirationDate { get; set; }
        public string Value { get; set; }
    }
}