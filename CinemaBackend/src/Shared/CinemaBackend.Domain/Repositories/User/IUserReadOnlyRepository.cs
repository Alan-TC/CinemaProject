namespace CinemaBackend.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<bool> UserEmailExists(string email);
}
