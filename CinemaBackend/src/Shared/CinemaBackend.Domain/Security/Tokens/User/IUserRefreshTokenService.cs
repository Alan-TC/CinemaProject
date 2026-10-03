namespace CinemaBackend.Domain.Security.Tokens.User;

public interface IUserRefreshTokenService
{
    Task<bool> Validate(string refreshToken);
    Guid? GetGuid(string refreshToken);
}
