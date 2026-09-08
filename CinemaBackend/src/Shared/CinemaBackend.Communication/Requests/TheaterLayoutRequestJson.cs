using CinemaBackend.Communication.SharedRequestResponse;

namespace CinemaBackend.Communication.Requests;

public class TheaterLayoutRequestJson
{
    public string Tag { get; set; } = string.Empty;
    public LayoutGridJson Layout { get; set; } = default!;
}
