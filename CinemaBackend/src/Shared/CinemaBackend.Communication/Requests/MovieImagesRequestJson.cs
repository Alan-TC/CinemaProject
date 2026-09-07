namespace CinemaBackend.Communication.Requests;

public class MovieImagesRequestJson
{
    public Guid MovieId { get; set; }
    public Stream? Poster { get; set; }
    public Stream? FeaturedBanner { get; set; }
    public Stream? StandardBanner { get; set; }
}
