using CinemaBackend.Communication.Enums;

namespace CinemaBackend.Communication.Responses.Session;

public class SeatReponseJson
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public int PosX { get; set; }
    public int PosY { get; set;}
    public SeatStatus Status { get; set; }
}
