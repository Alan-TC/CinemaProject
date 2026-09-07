using CinemaBackend.Domain.DTOs;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Domain.Repositories.Movie;

public interface IMovieReadOnlyRepository
{
    Task<Entities.Movie?> GetMovieById(Guid id);

    Task<PaginationReturn<MovieInfoDto>> GetFilteredPagedMovies(PaginationQuery query);
}
