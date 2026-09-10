using CinemaBackend.Admin.Application.UseCases.Movie.Create;
using CinemaBackend.Domain.DTOs.Movie;
using CinemaBackend.SharedKernel.Extensions;

namespace CinemaBackend.Admin.Application.Mappings;

internal static class CreateMovieUseCaseRequestExtensions
{
    internal static UploadMovieImagesDto MapToUploadMovieDto(this CreateMovieUseCaseRequest request)
    {
        ImageInfo? poster = null;
        ImageInfo? featured = null;
        ImageInfo? standard = null;

        if (request.Poster != null)
            poster = new ImageInfo(request.Poster.ImageContentType()!, request.Poster);

        if (request.FeaturedBanner != null)
            featured = new ImageInfo(request.FeaturedBanner.ImageContentType()!, request.FeaturedBanner);

        if (request.StandardBanner != null)
            standard = new ImageInfo(request.StandardBanner.ImageContentType()!, request.StandardBanner);

        return new UploadMovieImagesDto(poster, featured, standard);
    }
}
