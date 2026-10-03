using CinemaBackend.Domain.Entities.UserEntities;
using CinemaBackend.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data.Repositories;

internal class UserRepository(CinemaDbContext dbContext) : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private readonly CinemaDbContext _dbContext = dbContext;

    public async Task<bool> UserEmailExists(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email && u.DeactivatedAt == null);
    }

    public async Task AddUser(User user)
    {
        await _dbContext.Users.AddAsync(user);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email && u.DeactivatedAt == null);
    }

    public async Task<User?> GetUserById(Guid id)
    {
        return await _dbContext
            .Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id && u.DeactivatedAt == null);
    }
}
