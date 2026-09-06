namespace CinemaBackend.Domain.DTOs;

public record UploadMovieImagesDto(
    ImageInfo? Poster = null,
    ImageInfo? FeaturedBanner = null,
    ImageInfo? StandardBanner = null
);

public record ImageInfo(
    string ContentType,
    Stream ImageStream
);
