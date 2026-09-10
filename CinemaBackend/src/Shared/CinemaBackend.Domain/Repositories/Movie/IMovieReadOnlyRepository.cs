using CinemaBackend.Domain.DTOs;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Domain.Repositories.Movie;

public interface IMovieReadOnlyRepository
{
    Task<Entities.MovieEntities.Movie?> GetMovieById(Guid id);

    Task<PaginationReturn<MovieInfoDto>> GetFilteredPagedMovies(PaginationQuery query, MovieFilterDto filter);
}
