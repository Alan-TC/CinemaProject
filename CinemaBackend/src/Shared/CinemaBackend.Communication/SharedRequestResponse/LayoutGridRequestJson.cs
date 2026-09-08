namespace CinemaBackend.Communication.SharedRequestResponse;

public class LayoutGridJson
{
    public int Width { get; set; }
    public int Height { get; set; }
    public List<GridCell> Grid { get; set; } = [];
}
