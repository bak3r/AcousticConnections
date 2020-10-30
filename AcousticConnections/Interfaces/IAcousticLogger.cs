using System;
using System.Net;

namespace AcousticConnections.Interfaces
{
    public interface IAcousticLogger
    {
        void Fatal(string message);
        void Error(string message);
        void Fatal(string message, WebException webException);
        void Fatal(string message, Exception exception);
    }
}