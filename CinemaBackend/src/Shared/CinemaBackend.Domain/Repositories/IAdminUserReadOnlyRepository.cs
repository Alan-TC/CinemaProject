using CinemaBackend.Domain.Entities;

namespace CinemaBackend.Domain.Repositories;

public interface IAdminUserReadOnlyRepository
{
    Task<AdminUser?> GetAdminUserByEmail(string email);
}
