using CinemaBackend.Admin.Application.Mappings;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.DTOs.Movie;
using CinemaBackend.Domain.DTOs.Shared;
using CinemaBackend.Domain.Enums;
using CinemaBackend.Domain.Repositories.Movie;
using CinemaBackend.Domain.Storage.Movie;
using CinemaBackend.SharedKernel.Pagination;

namespace CinemaBackend.Admin.Application.UseCases.Movie.GetMovies;

public class MoviesGetFilteredPagedUseCase(
    IMovieReadOnlyRepository movieReadOnlyRepository,
    IMovieImageStorage movieImageStorage
) : IMoviesGetFilteredPagedUseCase
{
    private readonly IMovieReadOnlyRepository _movieReadOnlyRepository = movieReadOnlyRepository;
    private readonly IMovieImageStorage _movieImageStorage = movieImageStorage;

    public async Task<PaginationReturn<MovieResponseJson>> Execute(PaginationQueryRequest query, MovieFilterRequestJson? filter)
    {
        var pageQuery = new PaginationQuery(Math.Max(1, query.CurrentPage), query.PageSize);

        var movieFilterDto = filter is null ? new MovieFilterDto() : 
            new MovieFilterDto
            {
                SearchTerm = filter.SearchTerm,
                ClassInd = (ClassInd?)filter.ClassInd,
                Genres = [.. filter.Genres.Select(g => (Genres)g)],
                ReleaseDate = filter.ReleaseDate is null ? null : 
                    new DateBetweenDto(filter.ReleaseDate.From, filter.ReleaseDate.To)
            };

        var totalList = await _movieReadOnlyRepository.GetFilteredPagedMovies(pageQuery, movieFilterDto);

        foreach (var movie in totalList.Items)
        {
            movie.ImagesUrl = _movieImageStorage.GetMovieImagesUrl(movie.Id);
        }

        var responseItems = totalList.Items.Select(x => x.MapToMovieResponseJson()).ToList();

        return new PaginationReturn<MovieResponseJson>(responseItems, totalList.CurrentPage, totalList.PageSize, totalList.TotalCount);
    }
}
