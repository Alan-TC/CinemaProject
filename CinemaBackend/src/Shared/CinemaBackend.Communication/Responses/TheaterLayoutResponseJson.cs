using CinemaBackend.Communication.SharedRequestResponse;

namespace CinemaBackend.Communication.Responses;

public class TheaterLayoutResponseJson
{
    public Guid Id { get; set; }
    public string Tag { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public LayoutGridJson Layout { get; set; } = default!;
}
