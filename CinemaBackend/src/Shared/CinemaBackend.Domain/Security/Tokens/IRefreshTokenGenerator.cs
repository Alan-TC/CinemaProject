namespace CinemaBackend.Domain.Security.Tokens;

public interface IRefreshTokenGenerator
{
    string Generate(string subClaim);
}