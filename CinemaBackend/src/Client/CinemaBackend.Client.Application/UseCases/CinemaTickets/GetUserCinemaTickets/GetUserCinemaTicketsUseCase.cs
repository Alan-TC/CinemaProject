using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses.CinemaTickets;
using CinemaBackend.Domain.Identity.Client;
using CinemaBackend.Domain.Repositories.CinemaTicket;
using CinemaBackend.SharedKernel.Pagination;
using CinemaBackend.Client.Application.Mappings;

namespace CinemaBackend.Client.Application.UseCases.CinemaTickets.GetUserCinemaTickets;

public class GetUserCinemaTicketsUseCase(
    ICinemaTicketReadOnlyRepository cinemaTicketReadOnlyRepository,
    ILoggedUser loggedUser) : IGetUserCinemaTicketsUseCase
{
    private readonly ICinemaTicketReadOnlyRepository _cinemaTicketReadOnlyRepository = cinemaTicketReadOnlyRepository;

    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<PaginationReturn<CinemaTicketResponseSummaryJson>> Execute(PaginationQueryRequest query)
    {
        var pageQuery = new PaginationQuery(Math.Max(1, query.CurrentPage), query.PageSize);

        var pagedList = await _cinemaTicketReadOnlyRepository.GetUserPagedList(pageQuery, _loggedUser.GetUserId());

        var items = pagedList.Items.Select(dto => dto.MapToCinemaTicketResponseSummaryJson()).ToList();

        return new(items, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalCount);
    }
}
