using Bogus;
using CinemaBackend.Communication.Requests;

namespace TestsCommonUtilities.Requests;

public class PaymentRequestJsonBuilder
{
    private readonly Faker<PaymentRequestJson> _faker = new();

    public PaymentRequestJsonBuilder()
    {
        _faker.RuleFor(r => r.SessionId, _ => Guid.CreateVersion7());

        _faker.RuleFor(r => r.Seats, f => f
            .Make(f.Random.Int(1, 5), _ => Guid.CreateVersion7()));

        _faker.RuleFor(r => r.FullPriceQuantity,
            (f, r) => f.Random.Int(0, r.Seats.Count));

        _faker.RuleFor(r => r.HalfPriceQuantity,
            (_, r) => r.Seats.Count - r.FullPriceQuantity);
    }

    public PaymentRequestJsonBuilder WithSessionId(Guid sessionId)
    {
        _faker.RuleFor(r => r.SessionId, sessionId);
        return this;
    }

    public PaymentRequestJsonBuilder WithFullPriceQuantity(int quantity)
    {
        _faker.RuleFor(r => r.FullPriceQuantity, quantity);
        return this;
    }
    
    public PaymentRequestJsonBuilder WithHalfPriceQuantity(int quantity)
    {
        _faker.RuleFor(r => r.HalfPriceQuantity, quantity);
        return this;
    }

    public PaymentRequestJsonBuilder WithSeats(List<Guid> seats)
    {
        _faker.RuleFor(r => r.FullPriceQuantity, seats.Count);
        _faker.RuleFor(r => r.HalfPriceQuantity, 0);
        _faker.RuleFor(r => r.Seats, seats);
        return this;
    }

    public PaymentRequestJsonBuilder WithTotalSeats(int totalSeats)
    {
        _faker.RuleFor(r => r.Seats, f => f
            .Make(totalSeats, _ => Guid.CreateVersion7()));
        
        return this;
    }

    public PaymentRequestJson Build() => _faker.Generate();
}
