namespace CinemaBackend.Domain.Entities.SessionEntities;

public class SessionRoom
{
    public Guid Id { get; set; }
    public string Tag { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }

    public ICollection<SessionRoomSeat> Seats { get; set; } = default!;

    public Guid SessionId { get; private set; }
}
