using Bogus;
using CinemaBackend.Communication.Requests;

namespace TestsCommonUtilities.Requests;

public class MovieImagesRequestJsonBuilder
{
    private readonly Faker<MovieImagesRequestJson> _faker = new();

    public MovieImagesRequestJsonBuilder()
    {
        _faker.RuleFor(r => r.MovieId, _ => Guid.CreateVersion7());
    }

    public MovieImagesRequestJsonBuilder WithMovieId(Guid id)
    {
        _faker.RuleFor(r => r.MovieId, id);
        return this;
    }

    public MovieImagesRequestJsonBuilder WithPoster(string fileType)
    {
        _faker.RuleFor(r => r.Poster, FileType.GetStream(fileType));
        return this;
    }

    public MovieImagesRequestJsonBuilder WithFeaturedBanner(string fileType)
    {
        _faker.RuleFor(r => r.FeaturedBanner, FileType.GetStream(fileType));
        return this;
    }

    public MovieImagesRequestJsonBuilder WithStandardBanner(string fileType)
    {
        _faker.RuleFor(r => r.StandardBanner, FileType.GetStream(fileType));
        return this;
    }

    public MovieImagesRequestJson Build() => _faker.Generate();

}
