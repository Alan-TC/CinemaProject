using CinemaBackend.Domain.Entities;
using CinemaBackend.Domain.Repositories;
using CinemaBackend.Infrastructure.Security.PasswordHasher;

namespace CinemaBackend.Infrastructure.Data.Repositories;

public class AdminUserRepository : IAdminUserReadOnlyRepository
{
    public async Task<AdminUser?> GetAdminUserByEmail(string email)
    {
        // TODO: Temporário
        return new AdminUser
        {
            Email = email,
            PasswordHash = new PasswordHasherArgon2id().HashPassword("password")
        };
    }
}
