using CinemaBackend.Communication.ResourceFiles.Errors;
using System.Net;

namespace CinemaBackend.Communication.Exceptions;

public class NotFoundException : CinemaBackendExceptionBase
{
    public override HttpStatusCode GetErrorCode() => HttpStatusCode.NotFound;

    public override List<string> GetErrorMessages() => [ResourceErrorMessages.RESOURCE_NOT_FOUND];
}
