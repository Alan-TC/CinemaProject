using CinemaBackend.Communication.Enums;

namespace CinemaBackend.Communication.Requests;

public class MovieFilterRequestJson
{
    public string? SearchTerm { get; set; }

    public IList<Genres> Genres { get; set; } = [];

    public DateBetweenRequest? ReleaseDate { get; set; }

    public ClassInd? ClassInd { get; set; }
}
