using CinemaBackend.Domain.DTOs.TheaterLayout;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Domain.Repositories.TheaterLayout;

public interface ITheaterLayoutReadOnlyRepository
{
    Task<bool> HasTheaterWithTag(string tag);

    Task<PaginationReturn<Entities.TheaterLayoutEntities.TheaterLayout>> GetFilteredPagedMovies(PaginationQuery query, TheaterLayoutFilterDto filter);
}
