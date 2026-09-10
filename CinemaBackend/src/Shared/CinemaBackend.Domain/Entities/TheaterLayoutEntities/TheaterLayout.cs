namespace CinemaBackend.Domain.Entities.TheaterLayoutEntities;

public class TheaterLayout : EntityBase
{
    public string Tag { get; set; } = string.Empty;
    public LayoutGrid Layout { get; set; } = default!;
}
