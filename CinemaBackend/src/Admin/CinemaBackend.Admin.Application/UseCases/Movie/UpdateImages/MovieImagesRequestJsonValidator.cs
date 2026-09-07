using CinemaBackend.Admin.Application.Shared;
using CinemaBackend.Communication.Requests;
using FluentValidation;

namespace CinemaBackend.Admin.Application.UseCases.Movie.UpdateImages;

public class MovieImagesRequestJsonValidator : AbstractValidator<MovieImagesRequestJson>
{
    public MovieImagesRequestJsonValidator()
    {
        RuleFor(r => r.Poster)
            .Image();

        RuleFor(r => r.StandardBanner)
            .Image();

        RuleFor(r => r.FeaturedBanner)
            .Image();
    }
}
