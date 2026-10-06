using Bogus;
using CinemaBackend.Domain.Entities.SessionEntities;

namespace TestsCommonUtilities.Entities;

public class SessionBuilder
{
    private readonly Faker<Session> _faker = new();
    
    private int wStart = 1;
    private int wEnd = 10;
    private int hStart = 1;
    private int hEnd = 10;

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
    
    public Session Build() => _faker.Generate();
} 