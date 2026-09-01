namespace CinemaBackend.SharedKernel.Responses;

public class ResponseErrorJson
{
    public IEnumerable<string> ErrorMessages { get; private set; } = [];
    public bool AccessTokoenExpired { get; private set; } = false;

    public ResponseErrorJson(IEnumerable<string> errorMessages) => ErrorMessages = errorMessages;

    public ResponseErrorJson(string errorMessage) => ErrorMessages = [errorMessage];

    public ResponseErrorJson(string errorMessage, bool accessTokenExpired)
    {
        ErrorMessages = [errorMessage];
        AccessTokoenExpired = accessTokenExpired;
    }
}
