using System.Net;

namespace CinemaBackend.Communication.Exceptions;

public class ConflicException : CinemaBackendExceptionBase
{
    private List<string> Messages;
    
    public ConflicException(string message) => Messages = [message];
    
    public override HttpStatusCode GetErrorCode() => HttpStatusCode.Conflict;

    public override List<string> GetErrorMessages() => Messages;
}