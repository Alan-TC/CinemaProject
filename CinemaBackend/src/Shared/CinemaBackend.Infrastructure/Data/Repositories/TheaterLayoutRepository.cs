using CinemaBackend.Domain.DTOs.TheaterLayout;
using CinemaBackend.Domain.Entities.TheaterLayoutEntities;
using CinemaBackend.Domain.Repositories.TheaterLayout;
using CinemaBackend.SharedKernel.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data.Repositories;

internal class TheaterLayoutRepository(CinemaDbContext dbContext) : ITheaterLayoutWriteOnlyRepository, ITheaterLayoutReadOnlyRepository
{
    private readonly CinemaDbContext _dbContext = dbContext;
    public async Task Add(TheaterLayout theaterLayout) => 
        await _dbContext.TheaterLayouts.AddAsync(theaterLayout);

    public async Task<TheaterLayout?> GetById(Guid id) => 
        await _dbContext.TheaterLayouts
            .AsNoTracking()
            .Include(tl => tl.Layout)
            .FirstOrDefaultAsync(tl => tl.Id == id);

    public async Task<PaginationReturn<TheaterLayout>> GetFilteredPagedTheaterLayouts(PaginationQuery query, TheaterLayoutFilterDto filter)
    {
        var filteredQuery = _dbContext.TheaterLayouts
            .AsNoTracking()
            .Where(t => t.DeactivatedAt == null);

        if (filter.SearchTerm != null)
        {
            var searchTerms = filter.SearchTerm.ToLower().Split(", ").Select(s => s.Trim());

            foreach (var searchTerm in searchTerms)
            {
                filteredQuery = filteredQuery
                    .Where(t => 
                        t.Tag.ToLower().Contains(searchTerm));
            }
        }
        
        var resultList = await filteredQuery.OrderByDescending(d => d.CreatedAt)
            .Include(t => t.Layout)
            .Skip((query.CurrentPage - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        var totalCount = await filteredQuery.CountAsync();
        
        return new(resultList, query.CurrentPage, query.PageSize, totalCount);
    }

    public async Task<PaginationReturn<TheaterLayoutSelectorItemDto>> GetSelectorTheaterLayouts(PaginationQuery query, TheaterLayoutSelectorFilterDto filter)
    {
        var filteredQuery = _dbContext.TheaterLayouts
            .AsNoTracking()
            .Where(t => t.DeactivatedAt == null);
        
        if (filter.Tag != null)
            filteredQuery = filteredQuery
                .Where(t => t.Tag.ToLower().Contains(filter.Tag.ToLower().Trim()));
        
        var items = await filteredQuery
            .OrderByDescending(d => d.CreatedAt)
            .Select(t => new TheaterLayoutSelectorItemDto
            {
                Id = t.Id,
                Tag = t.Tag
            })
            .Skip((query.CurrentPage - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();
        
        var totalCount = await filteredQuery
            .Select(m => m.Id)
            .CountAsync();
        
        return new(items, query.CurrentPage, query.PageSize, totalCount);
    }

    public async Task<bool> HasTheaterWithTag(string tag) => 
        await _dbContext.TheaterLayouts.AnyAsync(t => t.Tag == tag && t.DeactivatedAt == null);
}
