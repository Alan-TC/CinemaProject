using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Admin.Application.UseCases.Movie.GetMovieSelector;

public interface IGetMovieSelectorUseCase
{
    Task<PaginationReturn<MovieSelectorItemResponseJson>> Execute(PaginationQueryRequest query, MovieSelectorRequestJson? filter);
}
