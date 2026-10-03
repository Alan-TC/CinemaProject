namespace CinemaBackend.Domain.Security.Tokens.User;

public interface IUserAccessTokenGenerator
{
    string Generate(Entities.UserEntities.User user);
}
