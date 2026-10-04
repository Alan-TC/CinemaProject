using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses.Session;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Client.Application.UseCases.Sessions.GetSessions;

public interface IGetSessionsFilteredPagedUseCase
{
    Task<PaginationReturn<SessionClientSumaryResponseJson>> Execute(PaginationQueryRequest query, string? searchTerm);
}
