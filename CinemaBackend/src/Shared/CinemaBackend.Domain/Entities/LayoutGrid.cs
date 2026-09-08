using CinemaBackend.Domain.Enums;

namespace CinemaBackend.Domain.Entities;

public class LayoutGrid
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public int Width { get; set; }
    public int Height { get; set; }

    public Guid TheaterLayoutId { get; set; }
    public List<GridCell> Grid { get; set; } = [];
}

public class GridCell
{
    public int PosX { get; set; }
    public int PosY { get; set; }
    public CellType Type { get; set; }
}