using CinemaBackend.Domain.DTOs.Movie;

namespace CinemaBackend.Domain.Storage.Movie;

public interface IMovieImageStorage
{
    Task UploadMovieImagesAsync(Guid movieId, UploadMovieImagesDto images, CancellationToken cancellationToken = default);
    MovieImagesUrlDto GetMovieImagesUrl(Guid movieId);
}