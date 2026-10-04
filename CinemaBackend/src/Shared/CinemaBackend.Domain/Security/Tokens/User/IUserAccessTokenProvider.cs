namespace CinemaBackend.Domain.Security.Tokens.User;

public interface IUserAccessTokenProvider
{
    string GetToken();
}
