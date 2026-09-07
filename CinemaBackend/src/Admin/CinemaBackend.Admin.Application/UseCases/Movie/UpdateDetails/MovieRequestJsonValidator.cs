using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.ResourceFiles.Errors;
using FluentValidation;

namespace CinemaBackend.Admin.Application.UseCases.Movie.UpdateDetails;

public class MovieRequestJsonValidator : AbstractValidator<MovieRequestJson>
{
    public MovieRequestJsonValidator()
    {   
        RuleFor(r => r.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_TITLE_REQUIRED)
            .MaximumLength(256)
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_TITLE_MAX_LENGTH);
        
        RuleFor(r => r.Sinopse)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_SINOPSE_REQUIRED)
            .MaximumLength(2048)
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_SINOPSE_MAX_LENGTH);
        
        RuleFor(r => r.Duration)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_DURATION_REQUIRED);
        
        RuleFor(r => r.ReleaseDate)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_RELEASE_DATE_REQUIRED);

        RuleFor(r => r.ClassInd)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_CLASS_IND_REQUIRED);

        RuleFor(r => r.ClassInd)
            .IsInEnum()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_CLASS_IND_INVALID);

        RuleFor(r => r.FilmGenres)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_GENRES_REQUIRED);

        RuleForEach(r => r.FilmGenres)
            .IsInEnum()
            .WithMessage(ResourceErrorMessages.VALIDATION_MOVIE_GENRES_INVALID);
    }
}
