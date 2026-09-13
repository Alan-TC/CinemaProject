namespace CinemaBackend.Communication.Requests;

public class SessionRequestJson
{
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set;}
    public DateTime StartTicketSalesAt { get; set; }
    public decimal Pricing { get; set; } = default!;
    public Guid TheaterLayoutId { get; set; }
    public Guid MovieId { get; set; }
}
