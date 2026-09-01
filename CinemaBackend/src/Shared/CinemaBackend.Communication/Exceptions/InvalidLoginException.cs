using System.Net;
using CinemaBackend.Communication.Exception;

namespace CinemaBackend.Communication.Exceptions;

public class InvalidLoginException : CinemaBackendExceptionBase
{
    public override int GetErrorCode() => (int)HttpStatusCode.Unauthorized;
    public override IEnumerable<string> GetErrorsMessages() => ["Invalid login credentials"];
}
