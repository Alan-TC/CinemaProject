using CinemaBackend.Domain.Entities;
using CinemaBackend.Domain.Repositories.AdminUser;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data.Repositories;

internal class AdminUserRepository(CinemaDbContext dbContext) : IAdminUserReadOnlyRepository, IAdminUserWriteOnlyRepository
{
    private readonly CinemaDbContext _dbContext = dbContext;
    public async Task<AdminUser?> GetAdminUserByEmail(string email)
    {
        return await _dbContext.AdminUsers.FirstOrDefaultAsync(u => u.Email == email && u.DeactivatedAt == null);
    }

    public async Task AddAdminUser(AdminUser user) => await _dbContext.AdminUsers.AddAsync(user);

    public async Task<bool> AdminUserEmailExists(string email) => await _dbContext.AdminUsers.AnyAsync(u => u.Email == email && u.DeactivatedAt == null);
}
