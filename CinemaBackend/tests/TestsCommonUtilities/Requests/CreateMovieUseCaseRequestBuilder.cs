using Bogus;
using CinemaBackend.Admin.Application.UseCases.Movie.Create;

namespace TestsCommonUtilities.Requests;

public class CreateMovieUseCaseRequestBuilder
{
    private readonly Faker<CreateMovieUseCaseRequest> _faker = new();
    private readonly MovieCreateRequestJsonBuilder _movieRequestJsonBuilder = new();

    public CreateMovieUseCaseRequestBuilder WithTitle(string title)
    {
        _movieRequestJsonBuilder.WithTitle(title);
        return this;
    }

    public CreateMovieUseCaseRequestBuilder WithSinopse(string sinopse)
    {
        _movieRequestJsonBuilder.WithSinopse(sinopse);
        return this;
    }

    public CreateMovieUseCaseRequestBuilder WithPoster(string fileType)
    {
        _faker.RuleFor(x => x.Poster, FileType.GetStream(fileType));
        return this;
    }

    public CreateMovieUseCaseRequestBuilder WithFeaturedBanner(string fileType)
    {
        _faker.RuleFor(x => x.FeaturedBanner, FileType.GetStream(fileType));
        return this;
    }

    public CreateMovieUseCaseRequestBuilder WithStandardBanner(string fileType)
    {
        _faker.RuleFor(x => x.StandardBanner, FileType.GetStream(fileType));
        return this;
    }

    public CreateMovieUseCaseRequest Build()
    {
        var movieDetails = _movieRequestJsonBuilder.Build();

        _faker.RuleFor(x => x.MovieDetails, movieDetails);

        return _faker.Generate();
    }
}