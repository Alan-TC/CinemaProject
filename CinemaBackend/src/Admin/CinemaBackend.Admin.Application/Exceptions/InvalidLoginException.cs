using System.Net;
using CinemaBackend.Domain.Exception;

namespace CinemaBackend.Admin.Application.Exceptions;

public class InvalidLoginException : CinemaBackendExceptionBase
{
    public override int GetErrorCode() => (int)HttpStatusCode.Unauthorized;
    public override IEnumerable<string> GetErrorsMessages() => ["Invalid login credentials"];
}
