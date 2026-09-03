namespace CinemaBackend.Domain.Repositories.AdminUser;

public interface IAdminUserReadOnlyRepository
{
    Task<Entities.AdminUser?> GetAdminUserByEmail(string email);
    Task<Entities.AdminUser?> GetAdminUserById(Guid id);
    Task<bool> AdminUserEmailExists(string email);
}
