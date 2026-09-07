using CinemaBackend.Admin.Application.Mappings;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Domain.Repositories.Movie;
using CinemaBackend.Domain.Storage.Movie;

namespace CinemaBackend.Admin.Application.UseCases.Movie.UpdateImages;

public class MovieUpdateImagesUseCase(
    IMovieReadOnlyRepository movieReadOnlyRepository,
    IMovieImageStorage movieImageStorage) : IMovieUpdateImagesUseCase
{
    private readonly IMovieReadOnlyRepository _movieReadOnlyRepository = movieReadOnlyRepository;
    private readonly IMovieImageStorage _movieImageStorage = movieImageStorage;

    public async Task Execute(MovieImagesRequestJson request)
    {
        ValidateAndThrowOnFailures(request);
        
        var movie = await _movieReadOnlyRepository.GetMovieById(request.MovieId);

        if (movie == null)
            throw new NotFoundException();

        var uploadMovieDto = request.MapToUploadMovieDto();

        await _movieImageStorage.UploadMovieImagesAsync(movie.Id, uploadMovieDto);
    }
    
    private static void ValidateAndThrowOnFailures(MovieImagesRequestJson request)
    {
        var validator = new MovieImagesRequestJsonValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
            throw new ErrorOnValidationException([.. result.Errors.Select(error => error.ErrorMessage)]);
    }
}
