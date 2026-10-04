using CinemaBackend.Client.Application.Mappings;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses.Session;
using CinemaBackend.Domain.Repositories.Session;
using CinemaBackend.Domain.Storage.Movie;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Client.Application.UseCases.Sessions.GetSessions;

public class GetSessionsFilteredPagedUseCase(
    ISessionReadOnlyRepository sessionReadOnlyRepository,
    IMovieImageStorage movieImageStorage
) : IGetSessionsFilteredPagedUseCase
{
    private readonly ISessionReadOnlyRepository _sessionReadOnlyRepository = sessionReadOnlyRepository;
    private readonly IMovieImageStorage _movieImageStorage = movieImageStorage;

    public async Task<PaginationReturn<SessionClientSumaryResponseJson>> Execute(PaginationQueryRequest query, string? searchTerm)
    {
        var pageQuery = new PaginationQuery(Math.Max(1, query.CurrentPage), query.PageSize);

        var pagedList = await _sessionReadOnlyRepository.GetClientPagedList(pageQuery, searchTerm);

        var items = pagedList.Items
            .Select(s => s.MapToSessionClientSumaryResponseJson(_movieImageStorage))
            .ToList();

        return new(items, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalCount);
    }
}
