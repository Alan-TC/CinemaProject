using CinemaBackend.Domain.DTOs.Movie;
using CinemaBackend.Domain.Storage.Movie;
using Moq;
using Npgsql;
using static System.Net.WebRequestMethods;

namespace TestsCommonUtilities.Storage.Movie;

public class IMovieImageStorageBuilder
{
    private readonly Mock<IMovieImageStorage> _mock = new();

    public IMovieImageStorageBuilder GetMovieImagesUrl(CinemaBackend.Domain.Entities.MovieEntities.Movie movie)
    {
        var fakeUrl = $"https://faketesturl/assets/{movie.Id}/";
        var retDto = new MovieImagesUrlDto
            (
                PosterUrl: $"{fakeUrl}poster.jpeg",
                FeaturedBanner: $"{fakeUrl}featured-banner.jpeg",
                StandardBanner: $"{fakeUrl}standard-banner.jpeg"
            );

        _mock.Setup(m => m.GetMovieImagesUrl(movie.Id)).Returns(retDto);

        return this;
    }

    public IMovieImageStorage Build() => _mock.Object;
}
