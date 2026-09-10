using CinemaBackend.Admin.Application.Mappings;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.DTOs.TheaterLayout;
using CinemaBackend.Domain.Repositories.TheaterLayout;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Admin.Application.UseCases.TheaterLayout.GetTheaterLayouts;

public class GetFilteredTheaterLayoutsUseCase(
    ITheaterLayoutReadOnlyRepository theaterLayoutReadOnlyRepository
) : IGetFilteredTheaterLayoutsUseCase
{
    private readonly ITheaterLayoutReadOnlyRepository _theaterLayoutReadOnlyRepository = theaterLayoutReadOnlyRepository;

    public async Task<PaginationReturn<TheaterLayoutResponseJson>> Execute(PaginationQueryRequest query, TheaterLayoutFilterRequestJson? filter)
    {
        var pageQuery = new PaginationQuery(Math.Max(1, query.CurrentPage), query.PageSize);
        var tlFilterDto = filter is null ? new TheaterLayoutFilterDto() : new TheaterLayoutFilterDto { SearchTerm = filter.SearchTerm};

        var result = await _theaterLayoutReadOnlyRepository.GetFilteredPagedMovies(pageQuery, tlFilterDto);

        var items = result.Items.Select(x => x.MapToTheaterLayoutResponseJson()).ToList();

        return new (items, result.CurrentPage, result.PageSize, result.TotalCount);
    }
}
