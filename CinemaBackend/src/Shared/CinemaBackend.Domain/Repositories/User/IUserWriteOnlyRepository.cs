namespace CinemaBackend.Domain.Repositories.User;

public interface IUserWriteOnlyRepository
{
    Task AddUser(Domain.Entities.UserEntities.User user);
}
