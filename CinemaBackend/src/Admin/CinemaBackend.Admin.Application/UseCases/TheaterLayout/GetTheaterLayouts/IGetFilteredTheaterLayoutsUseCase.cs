using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Admin.Application.UseCases.TheaterLayout.GetTheaterLayouts;

public interface IGetFilteredTheaterLayoutsUseCase
{
    Task<PaginationReturn<TheaterLayoutResponseJson>> Execute(PaginationQueryRequest query, TheaterLayoutFilterRequestJson? filter);
}
