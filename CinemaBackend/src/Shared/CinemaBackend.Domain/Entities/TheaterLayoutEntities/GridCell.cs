using CinemaBackend.Domain.Enums;

namespace CinemaBackend.Domain.Entities.TheaterLayoutEntities;

public class GridCell
{
    public int PosX { get; set; }
    public int PosY { get; set; }
    public CellType Type { get; set; }
}
