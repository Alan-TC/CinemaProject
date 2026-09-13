using CinemaBackend.Domain.DTOs.Session;
using CinemaBackend.Domain.Entities.SessionEntities;
using CinemaBackend.Domain.Repositories.Session;
using CinemaBackend.SharedKernel.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data.Repositories;

internal class SessionRepository(CinemaDbContext dbContext) : ISessionWriteOnlyRepository, ISessionReadOnlyRepository
{
    private readonly CinemaDbContext _dbContext = dbContext;

    public async Task Add(Session session) =>
        await _dbContext.Sessions.AddAsync(session);

    public async Task<PaginationReturn<SessionAdminSummaryDto>> GetAdminPagedList(PaginationQuery query, string? code)
    {
        var filteredQuery = _dbContext.Sessions
            .AsNoTracking()
            .AsSplitQuery()
            .Where(s => s.DeactivatedAt == null);

        if (code != null)
            filteredQuery = filteredQuery.Where(s => s.Code == code);
        
        var items = filteredQuery
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => new SessionAdminSummaryDto
            {
               Id = s.Id,
               Code = s.Code,
               MovieTitle = s.Movie.Title,
               StartsAt = s.StartsAt,
               EndsAt = s.EndsAt,
               StartTicketSalesAt = s.StartTicketSalesAt,
               CreatedAt = s.CreatedAt
            })
            .Skip((query.CurrentPage - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();
        
        var totalCount = await filteredQuery
            .Select(s => s.Id)
            .CountAsync();
        
        return new(items, query.CurrentPage, query.PageSize, totalCount);
    }

    public async Task<Session?> GetByCode(string code) =>
        await _dbContext.Sessions
            .AsNoTracking()
            .AsSplitQuery()
            .Include(s => s.SessionRoom)
                .ThenInclude(sr => sr.Seats)
            .Include(s => s.Movie)
            .Include(s => s.Pricing)
            .FirstOrDefaultAsync(s => s.Code == code);

    public async Task<Session?> GetById(Guid id) =>
        await _dbContext.Sessions
            .AsNoTracking()
            .AsSplitQuery()
            .Include(s => s.SessionRoom)
                .ThenInclude(sr => sr.Seats)
            .Include(s => s.Movie)
            .Include(s => s.Pricing)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task<bool> HasSessionWithCode(string code) =>
        await _dbContext.Sessions.AnyAsync(s => s.Code == code);
}
