using Bogus;
using CinemaBackend.Admin.Application.UseCases.Movie.Create;
using TestsCommonUtilities.Files;

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
        _faker.RuleFor(x => x.Poster, GetStream(fileType));
        return this;
    }

    public CreateMovieUseCaseRequestBuilder WithFeaturedBanner(string fileType)
    {
        _faker.RuleFor(x => x.FeaturedBanner, GetStream(fileType));
        return this;
    }

    public CreateMovieUseCaseRequestBuilder WithStandardBanner(string fileType)
    {
        _faker.RuleFor(x => x.StandardBanner, GetStream(fileType));
        return this;
    }

    public CreateMovieUseCaseRequest Build()
    {
        var movieDetails = _movieRequestJsonBuilder.Build();

        _faker.RuleFor(x => x.MovieDetails, movieDetails);

        return _faker.Generate();
    }

    private static Stream? GetStream(string fileType)
    {
        return fileType switch
        {
            FileType.PNG => FileFactory.GetPNG(),
            FileType.JPEG => FileFactory.GetJPEG(),
            FileType.BMP => FileFactory.GetBMP(),
            FileType.TXT => FileFactory.GetTXT(),
            _ => null
        };
    }
}

public static class FileType
{
    public const string PNG = "png";
    public const string JPEG = "jpeg";
    public const string BMP = "bmp";
    public const string TXT = "txt";
}