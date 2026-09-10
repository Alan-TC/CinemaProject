namespace CinemaBackend.Domain.Entities.TheaterLayoutEntities;

public class LayoutGrid
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public int Width { get; set; }
    public int Height { get; set; }

    public Guid TheaterLayoutId { get; set; }
    public List<GridCell> Grid { get; set; } = [];
}