using System.Net;
using CinemaBackend.SharedKernel.Exception;

namespace CinemaBackend.SharedKernel.Exceptions;

public class InvalidLoginException : CinemaBackendExceptionBase
{
    public override int GetErrorCode() => (int)HttpStatusCode.Unauthorized;
    public override IEnumerable<string> GetErrorsMessages() => ["Invalid login credentials"];
}
