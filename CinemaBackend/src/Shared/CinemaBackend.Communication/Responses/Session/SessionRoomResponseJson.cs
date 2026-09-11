namespace CinemaBackend.Communication.Responses.Session;

public class SessionRoomResponseJson
{
    public string Tag { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }

    public List<SeatReponseJson> Seats { get; set; } = [];
}
