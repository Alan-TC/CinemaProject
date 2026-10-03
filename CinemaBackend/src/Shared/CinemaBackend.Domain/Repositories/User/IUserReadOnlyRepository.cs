namespace CinemaBackend.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<Entities.UserEntities.User?> GetUserByEmail(string email);
    Task<Entities.UserEntities.User?> GetUserById(Guid id);
    Task<bool> UserEmailExists(string email);
}
