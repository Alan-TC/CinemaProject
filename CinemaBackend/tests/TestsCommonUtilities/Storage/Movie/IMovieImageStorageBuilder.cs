using CinemaBackend.Domain.DTOs.Movie;
using CinemaBackend.Domain.Storage.Movie;
using Moq;

namespace TestsCommonUtilities.Storage.Movie;

public class IMovieImageStorageBuilder
{
    private readonly Mock<IMovieImageStorage> _mock = new();

    public IMovieImageStorageBuilder GetMovieImagesUrl(CinemaBackend.Domain.Entities.MovieEntities.Movie? movie = null)
    {
        if (movie == null)
        {
            _mock.Setup(m => m.GetMovieImagesUrl(It.IsAny<Guid>())).Returns(new MovieImagesUrlDto(null, null, null));
            return this;
        }

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
