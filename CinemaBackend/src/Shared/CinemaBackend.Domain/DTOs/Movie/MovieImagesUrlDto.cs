namespace CinemaBackend.Domain.DTOs.Movie;

public record MovieImagesUrlDto(
    string? PosterUrl,
    string? FeaturedBanner,
    string? StandardBanner
);
