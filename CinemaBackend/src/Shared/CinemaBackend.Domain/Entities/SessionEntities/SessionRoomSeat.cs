using CinemaBackend.Domain.Enums;

namespace CinemaBackend.Domain.Entities.SessionEntities;

public class SessionRoomSeat
{
    public Guid Id { get; set;}
    public string Code { get; set; } = string.Empty;
    public int PosX { get; set; }
    public int PosY { get; set;}
    public SeatStatus Status { get; set; }

    public Guid SessionRoomId { get; private set; }
}
