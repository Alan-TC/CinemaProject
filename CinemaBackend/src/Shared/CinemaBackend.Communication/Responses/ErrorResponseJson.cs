namespace CinemaBackend.Communication.Responses;

public class ErrorResponseJson
{
    public IEnumerable<string> ErrorMessages { get; private set; } = [];
    public bool AccessTokenExpired { get; private set; } = false;

    public ErrorResponseJson(IEnumerable<string> errorMessages) => ErrorMessages = errorMessages;

    public ErrorResponseJson(string errorMessage) => ErrorMessages = [errorMessage];

    public ErrorResponseJson(string errorMessage, bool accessTokenExpired)
    {
        ErrorMessages = [errorMessage];
        AccessTokenExpired = accessTokenExpired;
    }
}
