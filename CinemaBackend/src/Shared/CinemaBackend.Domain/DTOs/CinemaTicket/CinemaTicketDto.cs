namespace CinemaBackend.Domain.DTOs.CinemaTicket;

public class CinemaTicketDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string TheaterRoomTag { get; set; } = string.Empty;
    public DateTime StartsAt { get; set; }
    public List<string> SeatsCodes { get; set; } = [];
    public int HalfPriceTickets { get; set; }
    public Entities.MovieEntities.Movie Movie { get; set; } = default!;
}
