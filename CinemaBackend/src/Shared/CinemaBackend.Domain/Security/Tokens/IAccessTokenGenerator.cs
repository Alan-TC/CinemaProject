namespace CinemaBackend.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(string subClaim);
}
