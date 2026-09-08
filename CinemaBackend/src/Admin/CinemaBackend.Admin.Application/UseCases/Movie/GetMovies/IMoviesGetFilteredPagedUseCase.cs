using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Admin.Application.UseCases.Movie.GetMovies;

public interface IMoviesGetFilteredPagedUseCase
{
    Task<PaginationReturn<MovieResponseJson>> Execute(PaginationQueryRequest query, MovieFilterRequestJson? filter);
}
