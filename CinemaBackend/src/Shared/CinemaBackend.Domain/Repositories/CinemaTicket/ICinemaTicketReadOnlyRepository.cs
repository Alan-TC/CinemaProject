using CinemaBackend.Domain.DTOs.CinemaTicket;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Domain.Repositories.CinemaTicket;

public interface ICinemaTicketReadOnlyRepository
{
    Task<bool> HasTicketWithCode(string ticketCode);

    Task<PaginationReturn<CinemaTicketSummaryDto>> GetUserPagedList(PaginationQuery query, Guid userId);

    Task<CinemaTicketDto?> GetByIdFromUser(Guid ticketId, Guid userId);
}
