using CinemaBackend.Communication.ResourceFiles.Errors;
using System.Net;

namespace CinemaBackend.Communication.Exceptions;

public class InvalidRefreshTokenException : CinemaBackendExceptionBase
{
    public override HttpStatusCode GetErrorCode() => HttpStatusCode.Unauthorized;

    public override List<string> GetErrorMessages() => [ResourceErrorMessages.VALIDATION_REFRESH_TOKEN_INVALID];
}
