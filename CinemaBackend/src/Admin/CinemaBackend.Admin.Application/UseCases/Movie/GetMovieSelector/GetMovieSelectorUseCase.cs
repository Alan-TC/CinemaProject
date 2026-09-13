using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.DTOs.Movie;
using CinemaBackend.Domain.Repositories.Movie;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Admin.Application.UseCases.Movie.GetMovieSelector;

public class GetMovieSelectorUseCase(
    IMovieReadOnlyRepository movieReadOnlyRepository
) : IGetMovieSelectorUseCase
{
    private readonly IMovieReadOnlyRepository _movieReadOnlyRepository = movieReadOnlyRepository;

    public async Task<PaginationReturn<MovieSelectorItemResponseJson>> Execute(PaginationQueryRequest query, MovieSelectorRequestJson? filter)
    {
        var pageQuery = new PaginationQuery(Math.Max(1, query.CurrentPage), query.PageSize);
        var filterDto = new MovieSelectorFilterDto { Title = filter?.Title};

        var pagedList = await _movieReadOnlyRepository.GetSelectorMovies(pageQuery, filterDto);

        var items = pagedList.Items
            .Select(m => new MovieSelectorItemResponseJson
            {
                Id = m.Id,
                Title = m.Title,
                ReleaseDate = m.ReleaseDate
            })
            .ToList();

        return new(items, pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalCount);
    }
}
