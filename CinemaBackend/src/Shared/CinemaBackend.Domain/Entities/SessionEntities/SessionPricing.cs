namespace CinemaBackend.Domain.Entities.SessionEntities;

public class SessionPricing
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public decimal NormalPrice { get; set; }

    public Guid SessionId { get; private set; }
}
