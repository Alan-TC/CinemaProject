using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Admin.Application.UseCases.TheaterLayout.GetSelector;

public interface IGetTheaterLayoutSelectorUseCase
{
    Task<PaginationReturn<TheaterSelectorItemResponseJson>> Execute(PaginationQueryRequest query, TheaterSelectorFilterRequestJson? filter);
}
