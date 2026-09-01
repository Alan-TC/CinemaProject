namespace CinemaBackend.Domain.Repositories.AdminUser;

public interface IAdminUserReadOnlyRepository
{
    Task<Entities.AdminUser?> GetAdminUserByEmail(string email);
    Task<bool> AdminUserEmailExists(string email);
}
