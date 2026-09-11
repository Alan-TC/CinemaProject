namespace CinemaBackend.Communication.Responses.Session;

public class SessionResponseJson
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set;}
    public DateTime StartTicketSalesAt { get; set; }
    public decimal Pricing { get; set; } = default!;
    public  SessionRoomResponseJson SessionRoom { get; set; } = default!;
    public MovieResponseJson Movie { get; set; } = default!;
}
