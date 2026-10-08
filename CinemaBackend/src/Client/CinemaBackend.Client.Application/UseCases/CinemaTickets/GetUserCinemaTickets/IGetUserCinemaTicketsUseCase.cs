using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses.CinemaTickets;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Client.Application.UseCases.CinemaTickets.GetUserCinemaTickets;

public interface IGetUserCinemaTicketsUseCase
{
    Task<PaginationReturn<CinemaTicketResponseSummaryJson>> Execute(PaginationQueryRequest query);
}
