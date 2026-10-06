using System.Net;

namespace CinemaBackend.Communication.Exceptions;

public class ConflictException : CinemaBackendExceptionBase
{
    private List<string> Messages;
    
    public ConflictException(string message) => Messages = [message];
    
    public override HttpStatusCode GetErrorCode() => HttpStatusCode.Conflict;

    public override List<string> GetErrorMessages() => Messages;
}