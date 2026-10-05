using CinemaBackend.Client.Application.Mappings;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses.Session;
using CinemaBackend.Domain.Repositories.Session;
using CinemaBackend.Domain.Storage.Movie;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Client.Application.UseCases.Movies.GetMovieSessions;

public class GetMovieSessionsFilteredPagedUseCase(
    ISessionReadOnlyRepository sessionReadOnlyRepository,
    IMovieImageStorage movieImageStorage
) : IGetMovieSessionsFilteredPagedUseCase
{
    private readonly ISessionReadOnlyRepository _sessionReadOnlyRepository = sessionReadOnlyRepository;
    private readonly IMovieImageStorage _movieImageStorage = movieImageStorage;

    public async Task<PaginationReturn<SessionClientSumaryResponseJson>> Execute(PaginationQueryRequest query, Guid movieId)
    {
        var pageQuery = new PaginationQuery(Math.Max(1, query.CurrentPage), query.PageSize);

        var pagedList = await _sessionReadOnlyRepository.GetClientPagedList(pageQuery, movieId: movieId);

        var items = pagedList.Items
            .Select(s => s.MapToSessionClientSumaryResponseJson(_movieImageStorage))
            .ToList();

        return new(items, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalCount);
    }
}
