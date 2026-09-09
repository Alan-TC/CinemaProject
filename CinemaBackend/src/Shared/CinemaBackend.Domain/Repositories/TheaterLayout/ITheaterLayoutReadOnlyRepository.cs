using CinemaBackend.Domain.DTOs;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Domain.Repositories.TheaterLayout;

public interface ITheaterLayoutReadOnlyRepository
{
    Task<bool> HasTheaterWithTag(string tag);

    Task<PaginationReturn<Entities.TheaterLayout>> GetFilteredPagedMovies(PaginationQuery query, TheaterLayoutFilterDto filter);
}
