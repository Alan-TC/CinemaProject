using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Domain.Entities.MovieEntities;
using CinemaBackend.Domain.Enums;
using CinemaBackend.Domain.Repositories.Movie;

namespace CinemaBackend.Admin.Application.UseCases.Movie.UpdateDetails;

public class UpdateMovieDetailsUseCase(
    IMovieUpdateOnlyRepository movieUpdateOnlyRepository,
    IUnitOfWork unitOfWork) : IUpdateMovieDetailsUseCase
{
    private readonly IMovieUpdateOnlyRepository _movieUpdateOnlyRepository = movieUpdateOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(Guid movieId, MovieRequestJson request)
    {
        ValidateAndThrowOnFailures(request);

        var movie = await _movieUpdateOnlyRepository.GetMovieById(movieId);

        if (movie == null)
            throw new NotFoundException();

        movie.Title = request.Title;
        movie.Sinopse = request.Sinopse;
        movie.Duration = request.Duration;
        movie.ReleaseDate = request.ReleaseDate;
        movie.FilmGenres = [.. request.FilmGenres.Select(genre => new FilmGenre { Genre = (Genres)genre }) ];
        movie.ClassInd = (ClassInd)request.ClassInd;

        await _unitOfWork.Commit();
    }

    private static void ValidateAndThrowOnFailures(MovieRequestJson request)
    {
        var validator = new MovieRequestJsonValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
            throw new ErrorOnValidationException([.. result.Errors.Select(error => error.ErrorMessage) ]);
    }
}
