using Bogus;
using CinemaBackend.Domain.Entities.MovieEntities;
using CinemaBackend.Domain.Enums;

namespace TestsCommonUtilities.Entities;

public class MovieBuilder
{
    private readonly Faker<Movie> _faker = new();

    public MovieBuilder()
    {
        _faker.RuleFor(x => x.Title, f => f.Lorem.Sentence(3));
        _faker.RuleFor(x => x.Sinopse, f => f.Lorem.Sentence(10));
        _faker.RuleFor(x => x.ReleaseDate, f => f.Date.Between(new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
        _faker.RuleFor(x => x.Duration, f => f.Date.Timespan(TimeSpan.FromHours(5)));
        _faker.RuleFor(x => x.ClassInd, f => f.PickRandom<ClassInd>());
        _faker.RuleFor(x => x.FilmGenres, f => 
            [.. f.Make(2, () => new FilmGenre { Genre = f.PickRandom<Genres>() }).DistinctBy(filmeGenre => filmeGenre.Genre) ]);
    }

    public Movie Build() => _faker.Generate();
}
