namespace CinemaBackend.Communication.Responses.CinemaTickets;

public class CinemaTicketResponseJson
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string TheaterRoomTag { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public List<string> SeatsCodes { get; set; } = [];
    public int HalfPriceTickets { get; set; }
    public MovieResponseJson Movie { get; set; } = default!;
}
