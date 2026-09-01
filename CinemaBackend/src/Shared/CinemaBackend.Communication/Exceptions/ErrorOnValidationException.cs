using System.Net;
using CinemaBackend.Communication.Exception;

namespace CinemaBackend.Communication.Exceptions;

public class ErrorOnValidationException : CinemaBackendExceptionBase
{
    private IEnumerable<string> Errors;
    
    public ErrorOnValidationException(IEnumerable<string> errors) => Errors = errors;
    
    public override int GetErrorCode() => (int)HttpStatusCode.BadRequest;

    public override IEnumerable<string> GetErrorsMessages() => Errors;
}
