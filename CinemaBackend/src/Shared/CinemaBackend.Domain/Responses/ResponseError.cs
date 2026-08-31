namespace CinemaBackend.Domain.Responses;

public class ResponseError
{
    public IEnumerable<string> ErrorMessages { get; private set; } = [];
    public bool AccessTokoenExpired { get; private set; } = false;

    public ResponseError(IEnumerable<string> errorMessages) => ErrorMessages = errorMessages;

    public ResponseError(string errorMessage) => ErrorMessages = [errorMessage];

    public ResponseError(string errorMessage, bool accessTokenExpired)
    {
        ErrorMessages = [errorMessage];
        AccessTokoenExpired = accessTokenExpired;
    }
}
