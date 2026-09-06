using Bogus;
using CinemaBackend.Communication.Enums;
using CinemaBackend.Communication.Requests;

namespace TestsCommonUtilities.Requests;

public class MovieCreateRequestJsonBuilder
{
    private readonly Faker<MovieCreateRequestJson> _faker = new();

    public MovieCreateRequestJsonBuilder()
    {
        _faker.RuleFor(x => x.Title, f => f.Lorem.Sentence(3));
        _faker.RuleFor(x => x.Sinopse, f => f.Lorem.Sentence(10));
        _faker.RuleFor(x => x.ReleaseDate, f => f.Date.Between(new DateTime(2000, 1, 1), new DateTime(2026, 1, 1)));
        _faker.RuleFor(x => x.Duration, f => f.Date.Timespan(TimeSpan.FromHours(5)));
        _faker.RuleFor(x => x.ClassInd, f => f.PickRandom<ClassInd>());
        _faker.RuleFor(x => x.FilmGenres, f => [.. f.Make(2, () => f.PickRandom<Genres>()).Distinct()]);
    }

    public MovieCreateRequestJsonBuilder WithTitle(string title)
    {
        _faker.RuleFor(x => x.Title, title);
        return this;
    }

    public MovieCreateRequestJsonBuilder WithSinopse(string sinopse)
    {
        _faker.RuleFor(x => x.Sinopse, sinopse);
        return this;
    }

    public MovieCreateRequestJson Build() => _faker.Generate();
}
