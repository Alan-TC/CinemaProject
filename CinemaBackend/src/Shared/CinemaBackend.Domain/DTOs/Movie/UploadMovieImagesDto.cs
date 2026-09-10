namespace CinemaBackend.Domain.DTOs.Movie;

public record UploadMovieImagesDto(
    ImageInfo? Poster = null,
    ImageInfo? FeaturedBanner = null,
    ImageInfo? StandardBanner = null
);

public record ImageInfo(
    string ContentType,
    Stream ImageStream
);
