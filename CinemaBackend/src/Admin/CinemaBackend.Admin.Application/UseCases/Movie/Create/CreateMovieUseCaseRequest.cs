using CinemaBackend.Communication.Requests;

namespace CinemaBackend.Admin.Application.UseCases.Movie.Create;

public class CreateMovieUseCaseRequest
{
    public MovieRequestJson MovieDetails { get; set; } = default!;
    public Stream? Poster { get; set; }
    public Stream? FeaturedBanner { get; set; }
    public Stream? StandardBanner { get; set; }
}
