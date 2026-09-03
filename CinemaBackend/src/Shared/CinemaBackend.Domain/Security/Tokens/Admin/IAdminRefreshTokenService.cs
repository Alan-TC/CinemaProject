namespace CinemaBackend.Domain.Security.Tokens.Admin;

public interface IAdminRefreshTokenService
{
    Task<bool> Validate(string refreshToken);
    Guid? GetGuid(string refreshToken);
}
