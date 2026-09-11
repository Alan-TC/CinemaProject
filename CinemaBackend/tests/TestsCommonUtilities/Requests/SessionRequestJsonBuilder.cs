using Bogus;
using CinemaBackend.Communication.Requests;

namespace TestsCommonUtilities.Requests;

public class SessionRequestJsonBuilder
{
    private readonly Faker<SessionRequestJson> _faker = new();
    public SessionRequestJsonBuilder()
    {
        _faker.RuleFor(r => r.StartTicketSalesAt, 
            f => f.Date.Between(
                    new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc), 
                    new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)));

        _faker.RuleFor(r => r.StartsAt, (f, c) => c.StartTicketSalesAt.AddDays(f.Random.Number(5, 20)));

        _faker.RuleFor(r => r.EndsAt, (f, c) => c.StartsAt.AddMinutes(f.Random.Number(55, 240)));
        _faker.RuleFor(r => r.Pricing, f => f.Random.Decimal(1, 100));

        _faker.RuleFor(r => r.TheaterLayoutId, _ => Guid.CreateVersion7());
        _faker.RuleFor(r => r.MovieId, _ => Guid.CreateVersion7());
    }

    public SessionRequestJsonBuilder WithTheaterId(Guid id)
    {
        _faker.RuleFor(r => r.TheaterLayoutId, id);
        return this;
    }

    public SessionRequestJsonBuilder WithMovieId(Guid id)
    {
        _faker.RuleFor(r => r.MovieId, id);
        return this;
    }

    public SessionRequestJsonBuilder WithDates(DateTime startTickesSaleAt, DateTime startAt, DateTime endAt)
    {
        _faker.RuleFor(r => r.StartTicketSalesAt, startTickesSaleAt);

        _faker.RuleFor(r => r.StartsAt, startAt);

        _faker.RuleFor(r => r.EndsAt, endAt);

        return this;
    }

    public SessionRequestJsonBuilder WithPrice(decimal price)
    {
        _faker.RuleFor(r => r.Pricing, price);

        return this;
    }

    public SessionRequestJson Build() => _faker.Generate();
}
