using CinemaBackend.Domain.DTOs.Movie;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Domain.Repositories.Movie;

public interface IMovieReadOnlyRepository
{
    Task<Entities.MovieEntities.Movie?> GetMovieById(Guid id, bool onlyInActiveSessions = false);

    Task<PaginationReturn<MovieInfoDto>> GetFilteredPagedMovies(PaginationQuery query, MovieFilterDto filter, bool onlyInActiveSessions = false);

    Task<PaginationReturn<MovieSelectorItemDto>> GetSelectorMovies(PaginationQuery query, MovieSelectorFilterDto filter);
}
