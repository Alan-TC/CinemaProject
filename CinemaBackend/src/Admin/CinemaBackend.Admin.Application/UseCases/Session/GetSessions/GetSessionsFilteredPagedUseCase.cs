using CinemaBackend.Admin.Application.Mappings;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses.Session;
using CinemaBackend.Domain.Repositories.Session;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Admin.Application.UseCases.Session.GetSessions;

public class GetSessionsFilteredPagedUseCase(
    ISessionReadOnlyRepository sessionReadOnlyRepository
) : IGetSessionsFilteredPagedUseCase
{
    private readonly ISessionReadOnlyRepository _sessionReadOnlyRepository = sessionReadOnlyRepository;

    public async Task<PaginationReturn<SessionAdminSumaryResponseJson>> Execute(PaginationQueryRequest query, string? code)
    {
        var pageQuery = new PaginationQuery(Math.Max(1, query.CurrentPage), query.PageSize);

        var pagedList = await _sessionReadOnlyRepository.GetAdminPagedList(pageQuery, code);

        var items = pagedList.Items
            .Select(s => s.MapToSessionAdminSumaryResponseJson())
            .ToList();

        return new(items, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalCount);
    }
}
