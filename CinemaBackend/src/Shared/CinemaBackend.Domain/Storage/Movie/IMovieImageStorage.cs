using CinemaBackend.Domain.DTOs;

namespace CinemaBackend.Domain.Storage.Movie;

public interface IMovieImageStorage
{
    Task UploadMovieImagesAsync(Guid movieId, UploadMovieImagesDto images, CancellationToken cancellationToken = default);
    MovieImagesUrlDto GetMovieImagesUrl(Entities.Movie movie);
}