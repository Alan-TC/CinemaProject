using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses.Session;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Admin.Application.UseCases.Session.GetSessions;

public interface IGetSessionsFilteredPagedUseCase
{
    Task<PaginationReturn<SessionAdminSumaryResponseJson>> Execute(PaginationQueryRequest query, string? code);
}
