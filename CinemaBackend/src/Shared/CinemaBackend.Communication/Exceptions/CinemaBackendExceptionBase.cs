using System.Net;

namespace CinemaBackend.Communication.Exceptions;

public abstract class CinemaBackendExceptionBase : Exception
{
    public abstract List<string> GetErrorMessages();
    public abstract HttpStatusCode GetErrorCode();
}
