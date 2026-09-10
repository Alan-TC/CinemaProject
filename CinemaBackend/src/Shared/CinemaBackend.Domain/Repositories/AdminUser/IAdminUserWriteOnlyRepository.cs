namespace CinemaBackend.Domain.Repositories.AdminUser;

public interface IAdminUserWriteOnlyRepository
{
    Task AddAdminUser(Entities.AdminUserEntities.AdminUser user);
}
