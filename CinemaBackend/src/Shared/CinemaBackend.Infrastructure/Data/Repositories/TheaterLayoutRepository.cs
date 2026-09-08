using CinemaBackend.Domain.Entities;
using CinemaBackend.Domain.Repositories.TheaterLayout;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data.Repositories;

internal class TheaterLayoutRepository(CinemaDbContext dbContext) : ITheaterLayoutWriteOnlyRepository, ITheaterLayoutReadOnlyRepository
{
    private readonly CinemaDbContext _dbContext = dbContext;
    public async Task Add(TheaterLayout theaterLayout) => 
        await _dbContext.TheaterLayouts.AddAsync(theaterLayout);

    public async Task<bool> HasTheaterWithTag(string tag) => 
        await _dbContext.TheaterLayouts.AnyAsync(t => t.Tag == tag && t.DeactivatedAt == null);
}
