namespace CinemaBackend.SharedKernel.Exception;

public abstract class CinemaBackendExceptionBase : System.Exception
{
    public abstract IEnumerable<string> GetErrorsMessages();
    public abstract int GetErrorCode();
}
