using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses.Session;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Client.Application.UseCases.Movies.GetMovieSessions;

public interface IGetMovieSessionsFilteredPagedUseCase
{
    Task<PaginationReturn<SessionClientSumaryResponseJson>> Execute(PaginationQueryRequest query, Guid movieId);
}
