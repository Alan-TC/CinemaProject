using CinemaBackend.Domain.DTOs.Movie;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Domain.Repositories.Movie;

public interface IMovieReadOnlyRepository
{
    Task<Entities.MovieEntities.Movie?> GetMovieById(Guid id);

    Task<PaginationReturn<MovieInfoDto>> GetFilteredPagedMovies(PaginationQuery query, MovieFilterDto filter);

    Task<PaginationReturn<MovieSelectorItemDto>> GetSelectorMovies(PaginationQuery query, MovieSelectorFilterDto filter);
}
