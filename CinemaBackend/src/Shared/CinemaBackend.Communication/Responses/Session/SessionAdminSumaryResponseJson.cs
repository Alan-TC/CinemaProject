namespace CinemaBackend.Communication.Responses.Session;

public class SessionAdminSumaryResponseJson
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string MovieTitle { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set;}
    public DateTime StartTicketSalesAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
