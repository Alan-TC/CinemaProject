using System.Net;

namespace CinemaBackend.Communication.Exceptions;

public class InvalidLoginException : CinemaBackendExceptionBase
{
    public override HttpStatusCode GetErrorCode() => HttpStatusCode.Unauthorized;
    public override List<string> GetErrorMessages() => ["Invalid login credentials"];
}
