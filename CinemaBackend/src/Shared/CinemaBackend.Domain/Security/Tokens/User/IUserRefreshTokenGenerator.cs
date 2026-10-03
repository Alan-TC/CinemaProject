namespace CinemaBackend.Domain.Security.Tokens.User;

public interface IUserRefreshTokenGenerator
{
    string Generate(Entities.UserEntities.User adminUser);
}