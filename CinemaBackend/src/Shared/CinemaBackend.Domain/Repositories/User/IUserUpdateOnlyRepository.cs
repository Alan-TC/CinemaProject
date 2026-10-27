namespace CinemaBackend.Domain.Repositories.User;

public interface IUserUpdateOnlyRepository
{
    Task<Entities.UserEntities.User?> GetUserById(Guid id);
}
