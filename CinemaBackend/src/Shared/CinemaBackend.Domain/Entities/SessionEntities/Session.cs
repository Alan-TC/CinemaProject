using CinemaBackend.Domain.Entities.MovieEntities;

namespace CinemaBackend.Domain.Entities.SessionEntities;

public class Session : EntityBase
{
    public string Code { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set;}
    public DateTime StartTicketSalesAt { get; set; }

    public SessionPricing Pricing { get; set; } = default!;

    public Guid MovieId { get; private set; }
    public Movie Movie { get; set; } = default!;

    public SessionRoom SessionRoom { get; set; } = default!;
}
