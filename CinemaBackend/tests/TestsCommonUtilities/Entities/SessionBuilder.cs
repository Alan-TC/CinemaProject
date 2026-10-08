using Bogus;
using CinemaBackend.Domain.Entities.SessionEntities;

namespace TestsCommonUtilities.Entities;

public class SessionBuilder
{
    private readonly Faker<Session> _faker = new();

    public SessionBuilder()
    {
        _faker.RuleFor(e => e.Code, f => f.Random.AlphaNumeric(8));
        
        _faker.RuleFor(e => e.StartTicketSalesAt,
            f => f.Date.Between(
                new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)));

        _faker.RuleFor(e => e.StartsAt, (f, c) => c.StartTicketSalesAt.AddDays(f.Random.Number(5, 20)));

        _faker.RuleFor(e => e.EndsAt, (f, c) => c.StartsAt.AddMinutes(f.Random.Number(55, 240)));
        
        _faker.RuleFor(e => e.Pricing, f => new SessionPricing { NormalPrice = f.Random.Decimal(1, 100) });
        
        _faker.RuleFor(e => e.MovieId, f => f.Random.Guid());

        _faker.RuleFor(e => e.SessionRoom, f => new TheaterLayoutBuilder().Build().MapToSessionRoom());
    }

    public SessionBuilder WithDateUtcNow()
    {
        _faker.RuleFor(e => e.StartTicketSalesAt, f => DateTime.UtcNow.AddDays(f.Random.Number(-20, -5)));

        _faker.RuleFor(e => e.StartsAt, (f, c) => DateTime.UtcNow.AddDays(f.Random.Number(5, 20)));

        _faker.RuleFor(e => e.EndsAt, (f, c) => c.StartsAt.AddMinutes(f.Random.Number(120, 240)));

        return this;
    }

    public SessionBuilder WithStartTicketSalesAt(DateTime date)
    {
        _faker.RuleFor(e => e.StartTicketSalesAt, date);
        return this;
    }

    public SessionBuilder WithStartsAt(DateTime date)
    {
        _faker.RuleFor(e => e.StartsAt, date);
        return this;
    }

    public SessionBuilder WithEndsAt(DateTime date)
    {
        _faker.RuleFor(e => e.EndsAt, date);
        return this;
    }

    public Session Build() => _faker.Generate();
} 