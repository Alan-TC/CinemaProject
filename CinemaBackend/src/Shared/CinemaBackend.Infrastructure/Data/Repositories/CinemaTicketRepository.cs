using CinemaBackend.Domain.DTOs.CinemaTicket;
using CinemaBackend.Domain.Entities.CinemaTicketEntities;
using CinemaBackend.Domain.Repositories.CinemaTicket;
using CinemaBackend.SharedKernel.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data.Repositories;

internal class CinemaTicketRepository(CinemaDbContext dbContext) : ICinemaTicketReadOnlyRepository, ICinemaTicketWriteOnlyRepository
{
    private readonly CinemaDbContext _dbContext = dbContext;

    public async Task AddTicket(CinemaTicket ticket)
    {
        await _dbContext.CinemaTickets.AddAsync(ticket);
    }

    public Task<CinemaTicketDto?> GetByIdFromUser(Guid ticketId, Guid userId)
    {
        return _dbContext.CinemaTickets
            .AsNoTracking()
            .AsSplitQuery()
            .Where(e => e.Id == ticketId && e.DeactivatedAt == null && e.Payment.UserId == userId)
            .Include(e => e.Payment)
                .ThenInclude(e => e.Session)
                    .ThenInclude(e => e.Movie)
                        .ThenInclude(e => e.FilmGenres)
            .Select(e => new CinemaTicketDto
            {
                Id = e.Id,
                Code = e.Code,
                TheaterRoomTag = e.Payment.Session.SessionRoom.Tag,
                StartsAt = e.Payment.Session.StartsAt,
                HalfPriceTickets = e.Payment.HalfPriceQuantity,
                SeatsCodes = e.Payment.Seats.Select(s => s.SeatCode).ToList(),
                Movie = e.Payment.Session.Movie
            }).FirstOrDefaultAsync();
    }

    public async Task<PaginationReturn<CinemaTicketSummaryDto>> GetUserPagedList(PaginationQuery query, Guid userId)
    {
        var filteredQuery = _dbContext.CinemaTickets
            .AsNoTracking()
            .AsSplitQuery()
            .Where(e => e.DeactivatedAt == null && e.Payment.UserId == userId);

        var items = filteredQuery
            .OrderByDescending(e => e.CreatedAt)
            .Select(e => new CinemaTicketSummaryDto
            {
                Id = e.Id,
                Code = e.Code,
                TheaterRoomTag = e.Payment.Session.SessionRoom.Tag,
                StartsAt = e.Payment.Session.StartsAt,
                HalfPriceTickets = e.Payment.HalfPriceQuantity,
                SeatsCodes = e.Payment.Seats.Select(s => s.SeatCode).ToList()
            })
            .Skip((query.CurrentPage - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        var totalCount = await filteredQuery
            .Select(s => s.Id)
            .CountAsync();

        return new(items, query.CurrentPage, query.PageSize, totalCount);
    }

    public async Task<bool> HasTicketWithCode(string ticketCode)
    {
        return await _dbContext.CinemaTickets.AnyAsync(e => e.Code == ticketCode);
    }
}
