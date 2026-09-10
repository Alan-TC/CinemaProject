namespace CinemaBackend.Domain.Repositories.AdminUser;

public interface IAdminUserReadOnlyRepository
{
    Task<Entities.AdminUserEntities.AdminUser?> GetAdminUserByEmail(string email);
    Task<Entities.AdminUserEntities.AdminUser?> GetAdminUserById(Guid id);
    Task<bool> AdminUserEmailExists(string email);
}
