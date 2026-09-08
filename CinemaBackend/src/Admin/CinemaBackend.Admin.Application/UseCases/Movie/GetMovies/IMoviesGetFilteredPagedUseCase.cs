using CinemaBackend.Communication.Requests;
using CinemaBackend.Domain.DTOs;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Admin.Application.UseCases.Movie.GetMovies;

public interface IMoviesGetFilteredPagedUseCase
{
    Task<PaginationReturn<MovieInfoDto>> Execute(PaginationQueryRequest query, MovieFilterRequestJson? filter);
}
