using CinemaBackend.Admin.Application.Shared;
using CinemaBackend.Communication.ResourceFiles.Errors;
using FluentValidation;

namespace CinemaBackend.Admin.Application.UseCases.Movie.Create;

public class CreateMovieUseCaseRequestValidator : AbstractValidator<CreateMovieUseCaseRequest>
{
    public CreateMovieUseCaseRequestValidator()
    {
        RuleFor(r => r.MovieDetails)
            .NotNull()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_DETAILS_REQUIRED);
        
        RuleFor(r => r.MovieDetails.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_TITLE_REQUIRED)
            .MaximumLength(256)
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_TITLE_MAX_LENGTH);
        
        RuleFor(r => r.MovieDetails.Sinopse)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_SINOPSE_REQUIRED)
            .MaximumLength(2048)
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_SINOPSE_MAX_LENGTH);
        
        RuleFor(r => r.MovieDetails.Duration)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_DURATION_REQUIRED);
        
        RuleFor(r => r.MovieDetails.ReleaseDate)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_RELEASE_DATE_REQUIRED);

        RuleFor(r => r.MovieDetails.ClassInd)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_CLASS_IND_REQUIRED);

        RuleFor(r => r.MovieDetails.ClassInd)
            .IsInEnum()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_CLASS_IND_INVALID);

        RuleFor(r => r.MovieDetails.FilmGenres)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_GENRES_REQUIRED);

        RuleForEach(r => r.MovieDetails.FilmGenres)
            .IsInEnum()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_GENRES_INVALID);
        
        RuleFor(r => r.Poster)
            .Image();

        RuleFor(r => r.StandardBanner)
            .Image();

        RuleFor(r => r.FeaturedBanner)
            .Image();
    }
}
