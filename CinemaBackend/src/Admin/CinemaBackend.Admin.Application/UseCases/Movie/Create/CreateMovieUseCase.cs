using CinemaBackend.Admin.Application.Mappings;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.Repositories.Movie;
using CinemaBackend.Domain.Storage.Movie;

namespace CinemaBackend.Admin.Application.UseCases.Movie.Create;

public class CreateMovieUseCase(
    IMovieWriteOnlyRepository movieWriteOnlyRepository,
    IMovieImageStorage movieStorage,
    IUnitOfWork unitOfWork) : ICreateMovieUseCase
{
    private readonly IMovieWriteOnlyRepository _movieWriteOnlyRepository = movieWriteOnlyRepository;
    private readonly IMovieImageStorage _movieStorage = movieStorage;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<MovieResponseJson> Execute(CreateMovieUseCaseRequest request)
    {
        ValidateAndThrowOnFailures(request);

        var movie = request.MovieDetails.MapToMovie();

        var uploadMovieDto = request.MapToUploadMovieDto();

        await _movieWriteOnlyRepository.Add(movie);

        await _unitOfWork.Commit();

        await _movieStorage.UploadMovieImagesAsync(movie.Id, uploadMovieDto);

        return movie.MapToMovieResponseJson(_movieStorage);
    }

    private static void ValidateAndThrowOnFailures(CreateMovieUseCaseRequest request)
    {
        var validator = new CreateMovieUseCaseRequestValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
            throw new ErrorOnValidationException([.. result.Errors.Select(e => e.ErrorMessage)]);
    }
}
