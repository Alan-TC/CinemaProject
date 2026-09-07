using CinemaBackend.Communication.Requests;
using CinemaBackend.Domain.DTOs;
using CinemaBackend.Domain.Repositories.Movie;
using CinemaBackend.Domain.Storage.Movie;
using CinemaBackend.SharedKernel.Pagination;
using FluentValidation.Validators;

namespace CinemaBackend.Admin.Application.UseCases.Movie.GetMovies;

public class MoviesGetFilteredPagedUseCase(
    IMovieReadOnlyRepository movieReadOnlyRepository,
    IMovieImageStorage movieImageStorage
) : IMoviesGetFilteredPagedUseCase
{
    private readonly IMovieReadOnlyRepository _movieReadOnlyRepository = movieReadOnlyRepository;
    private readonly IMovieImageStorage _movieImageStorage = movieImageStorage;

    public async Task<PaginationReturn<MovieInfoDto>> Execute(PaginationQueryRequest query)
    {
        var pageQuery = new PaginationQuery(Math.Max(1, query.CurrentPage), query.PageSize);

        var totalList = await _movieReadOnlyRepository.GetFilteredPagedMovies(pageQuery);

        foreach (var movie in totalList.Items)
        {
            movie.ImagesUrl = _movieImageStorage.GetMovieImagesUrl(movie.Id);
        }

        return totalList;
    }

}
