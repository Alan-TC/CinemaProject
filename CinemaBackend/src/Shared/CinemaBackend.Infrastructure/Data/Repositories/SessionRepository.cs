using CinemaBackend.Domain.Entities.SessionEntities;
using CinemaBackend.Domain.Repositories.Session;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data.Repositories;

internal class SessionRepository(CinemaDbContext dbContext) : ISessionWriteOnlyRepository, ISessionReadOnlyRepository
{
    private readonly CinemaDbContext _dbContext = dbContext;

    public async Task Add(Session session) =>
        await _dbContext.Sessions.AddAsync(session);

    public async Task<Session?> GetByCode(string code) =>
        await _dbContext.Sessions
            .AsNoTracking()
            .AsSplitQuery()
            .Include(s => s.SessionRoom)
                .ThenInclude(sr => sr.Seats)
            .Include(s => s.Movie)
            .Include(s => s.Pricing)
            .FirstOrDefaultAsync(s => s.Code == code);

    public async Task<bool> HasSessionWithCode(string code) =>
        await _dbContext.Sessions.AnyAsync(s => s.Code == code);
}
