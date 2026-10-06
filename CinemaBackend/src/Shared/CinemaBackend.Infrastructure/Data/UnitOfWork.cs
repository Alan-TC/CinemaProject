using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data;

internal class UnitOfWork(CinemaDbContext dbContext) : IUnitOfWork
{
    private readonly CinemaDbContext _dbContext = dbContext;

    public async Task<bool> Commit()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }

        return true;
    } 
}
