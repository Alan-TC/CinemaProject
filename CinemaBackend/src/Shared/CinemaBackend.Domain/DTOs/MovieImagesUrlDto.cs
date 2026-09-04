namespace CinemaBackend.Domain.DTOs;

public record MovieImagesUrlDto(
    string PosterUrl,
    string FeaturedBanner,
    string StandardBanner
);
