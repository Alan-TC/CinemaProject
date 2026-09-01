namespace CinemaBackend.Infrastructure.Data;

internal class UnitOfWork(CinemaDbContext dbContext) : IUnitOfWork
{
    private readonly CinemaDbContext _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
