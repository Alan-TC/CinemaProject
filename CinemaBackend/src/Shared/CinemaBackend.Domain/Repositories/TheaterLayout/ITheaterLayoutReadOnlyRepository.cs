using CinemaBackend.Domain.DTOs.TheaterLayout;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Domain.Repositories.TheaterLayout;

public interface ITheaterLayoutReadOnlyRepository
{
    Task<bool> HasTheaterWithTag(string tag);
    Task<Entities.TheaterLayoutEntities.TheaterLayout?> GetById(Guid id);
    Task<PaginationReturn<Entities.TheaterLayoutEntities.TheaterLayout>> GetFilteredPagedTheaterLayouts(PaginationQuery query, TheaterLayoutFilterDto filter);
    Task<PaginationReturn<TheaterLayoutSelectorItemDto>> GetSelectorTheaterLayouts(PaginationQuery query, TheaterLayoutSelectorFilterDto filter);
}
