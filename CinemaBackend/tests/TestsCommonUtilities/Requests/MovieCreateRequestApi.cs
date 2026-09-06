using CinemaBackend.Communication.Requests;

namespace TestsCommonUtilities.Requests;

public class MovieCreateRequestApi
{
    public MovieCreateRequestJson MovieDetails { get; set; } = default!;
    public Stream? Poster { get; set; }
    public Stream? FeaturedBanner { get; set; }
    public Stream? StandardBanner { get; set; }
}