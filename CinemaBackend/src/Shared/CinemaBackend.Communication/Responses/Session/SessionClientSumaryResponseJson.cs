using CinemaBackend.Communication.Enums;

namespace CinemaBackend.Communication.Responses.Session;

public class SessionClientSumaryResponseJson
{
    public Guid Id { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public ClassInd ClassInd { get; set; }
    public string? MoviePosterUrl { get; set; } = null;
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set;}
}
