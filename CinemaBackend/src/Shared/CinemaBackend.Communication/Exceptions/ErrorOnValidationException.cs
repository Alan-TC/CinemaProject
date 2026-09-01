using System.Net;

namespace CinemaBackend.Communication.Exceptions;

public class ErrorOnValidationException : CinemaBackendExceptionBase
{
    private readonly List<string> Messages;
    
    public ErrorOnValidationException(List<string> errors) => Messages = errors;
    
    public override HttpStatusCode GetErrorCode() => HttpStatusCode.BadRequest;

    public override List<string> GetErrorMessages() => Messages;
}
