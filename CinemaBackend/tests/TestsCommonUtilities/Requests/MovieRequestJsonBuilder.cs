using Bogus;
using CinemaBackend.Communication.Enums;
using CinemaBackend.Communication.Requests;

namespace TestsCommonUtilities.Requests;

public class MovieRequestJsonBuilder
{
    private readonly Faker<MovieRequestJson> _faker = new();

    public MovieRequestJsonBuilder()
    {
        _faker.RuleFor(x => x.Title, f => f.Lorem.Sentence(3));
        _faker.RuleFor(x => x.Sinopse, f => f.Lorem.Sentence(10));
        _faker.RuleFor(x => x.ReleaseDate, f => f.Date.Between(new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
        _faker.RuleFor(x => x.Duration, f => f.Date.Timespan(TimeSpan.FromHours(5)));
        _faker.RuleFor(x => x.ClassInd, f => f.PickRandom<ClassInd>());
        _faker.RuleFor(x => x.FilmGenres, f => [.. f.Make(2, () => f.PickRandom<Genres>()).Distinct()]);
    }

    public MovieRequestJsonBuilder WithTitle(string title)
    {
        _faker.RuleFor(x => x.Title, title);
        return this;
    }

    public MovieRequestJsonBuilder WithSinopse(string sinopse)
    {
        _faker.RuleFor(x => x.Sinopse, sinopse);
        return this;
    }

    public MovieRequestJsonBuilder WithClassInd(ClassInd classInd)
    {
        _faker.RuleFor(x => x.ClassInd, classInd);
        return this;
    }

    public MovieRequestJsonBuilder WithFilmGenres(List<Genres> genres)
    {
        _faker.RuleFor(x => x.FilmGenres, genres);
        return this;
    }

    public MovieRequestJson Build() => _faker.Generate();
}
