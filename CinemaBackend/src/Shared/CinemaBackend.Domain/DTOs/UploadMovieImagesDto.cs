namespace CinemaBackend.Domain.DTOs;

public record UploadMovieImagesDto(
    ImageInfos? Poster = null,
    ImageInfos? FeaturedBanner = null,
    ImageInfos? StandardBanner = null
);

public record ImageInfos(
    string ContentType,
    Stream ImageStream
);
