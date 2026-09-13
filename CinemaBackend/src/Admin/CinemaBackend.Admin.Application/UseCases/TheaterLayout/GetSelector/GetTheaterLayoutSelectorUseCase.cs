using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.DTOs.TheaterLayout;
using CinemaBackend.Domain.Repositories.TheaterLayout;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Admin.Application.UseCases.TheaterLayout.GetSelector;

public class GetTheaterLayoutSelectorUseCase(
    ITheaterLayoutReadOnlyRepository theaterLayoutReadOnlyRepository
) : IGetTheaterLayoutSelectorUseCase
{
    private readonly ITheaterLayoutReadOnlyRepository _theaterLayoutReadOnlyRepository = theaterLayoutReadOnlyRepository;

    public async Task<PaginationReturn<TheaterSelectorItemResponseJson>> Execute(PaginationQueryRequest query, TheaterSelectorFilterRequestJson? filter)
    {
        var pageQuery = new PaginationQuery(Math.Max(1, query.CurrentPage), query.PageSize);
        var filterDto = new TheaterLayoutSelectorFilterDto { Tag = filter?.Tag };

        var pagedList = await _theaterLayoutReadOnlyRepository.GetSelectorTheaterLayouts(pageQuery, filterDto);

        var items = pagedList.Items.Select(t => new TheaterSelectorItemResponseJson
        {
            Id = t.Id,
            Tag = t.Tag
        }).ToList();
        
        return new(items, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalCount);
    }
}
