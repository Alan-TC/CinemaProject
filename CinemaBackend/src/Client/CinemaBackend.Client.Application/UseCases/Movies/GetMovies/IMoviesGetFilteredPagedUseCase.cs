using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Client.Application.UseCases.Movies.GetMovies;

public interface IMoviesGetFilteredPagedUseCase
{
    Task<PaginationReturn<MovieResponseJson>> Execute(PaginationQueryRequest query, MovieFilterRequestJson? filter);
}
