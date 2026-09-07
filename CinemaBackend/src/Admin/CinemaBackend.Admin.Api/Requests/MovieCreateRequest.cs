using CinemaBackend.Admin.Application.UseCases.Movie.Create;
using CinemaBackend.Communication.Requests;

namespace CinemaBackend.Admin.Api.Requests;

public class MovieCreateRequest
{
    public MovieRequestJson MovieDetails { get; set; } = default!;
    public IFormFile? Poster { get; set; }
    public IFormFile? FeaturedBanner { get; set; }
    public IFormFile? StandardBanner { get; set; }

    public CreateMovieUseCaseRequest MapToCreateMovieUseCaseRequest()
    {
        var request = new CreateMovieUseCaseRequest
        {
            MovieDetails = MovieDetails
        };

        if (Poster != null)
            request.Poster = Poster.OpenReadStream();

        if (FeaturedBanner != null)
            request.FeaturedBanner = FeaturedBanner.OpenReadStream();
        
        if (StandardBanner != null)
            request.StandardBanner = StandardBanner.OpenReadStream();
        
        return request;
    }
}
