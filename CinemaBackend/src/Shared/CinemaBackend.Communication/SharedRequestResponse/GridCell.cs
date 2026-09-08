using CinemaBackend.Communication.Enums;

namespace CinemaBackend.Communication.SharedRequestResponse;

public class GridCell
{
    public int PosX { get; set; }
    public int PosY { get; set; }
    public CellType Type { get; set; }
}
