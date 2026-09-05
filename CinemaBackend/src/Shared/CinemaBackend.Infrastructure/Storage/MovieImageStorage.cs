using CinemaBackend.Domain.DTOs;
using CinemaBackend.Domain.Storage.Movie;

namespace CinemaBackend.Infrastructure.Storage;

public class MovieImageStorage(IHostInfoProvider hostProvider) : IMovieImageStorage
{
    private readonly IHostInfoProvider _hostProvider = hostProvider;
    private const string MOVIE_STORAGE_PATH = "movies";
    private const string MOVIE_POSTER_NAME = "poster";
    private const string MOVIE_FEATURED_BANNER_NAME = "featured-banner";
    private const string MOVIE_STANDARD_BANNER_NAME = "standard-banner";

    public MovieImagesUrlDto GetMovieImagesUrl(Guid movieId)
    {
        var fileExtensionPattern = ".*";
        string? posterUrl = null;
        string? featuredBannerUrl = null;
        string? standardBannerUrl = null;

        string movieImagesRootPath = Path.Combine(_hostProvider.GetStorageRootPath(), MOVIE_STORAGE_PATH, movieId.ToString());

        string? post = MovieImageFind(movieImagesRootPath, MOVIE_POSTER_NAME + fileExtensionPattern);
        if (post != null)
            posterUrl = GetImageUrl(post, movieId);
        
        string? featured = MovieImageFind(movieImagesRootPath, MOVIE_FEATURED_BANNER_NAME + fileExtensionPattern);
        if (featured != null)
            featuredBannerUrl = GetImageUrl(featured, movieId);

        string? standard = MovieImageFind(movieImagesRootPath, MOVIE_STANDARD_BANNER_NAME + fileExtensionPattern);
        if (standard != null)
            standardBannerUrl = GetImageUrl(standard, movieId);

        return new MovieImagesUrlDto(
            PosterUrl: posterUrl,
            FeaturedBanner: featuredBannerUrl, 
            StandardBanner: standardBannerUrl);
    }

    private string GetImageUrl(string imageFullPath, Guid movieId)
    {
        return _hostProvider.GetRootUrl() + _hostProvider.GetStorageRequestPath() + "/"
            + MOVIE_STORAGE_PATH + "/" + movieId.ToString() + "/" + Path.GetFileName(imageFullPath);
    }

    private static string? MovieImageFind(string path, string pattern)
    {
        try
        {
            string[] paths = Directory.GetFiles(path, pattern);

            return paths.Length > 0 ? paths[0] : null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task UploadMovieImagesAsync(Guid movieId, UploadMovieImagesDto images, CancellationToken cancellationToken = default)
    {
        if (images.Poster != null)
            await UploadImage(movieId, MOVIE_POSTER_NAME, images.Poster);
        
        if (images.FeaturedBanner != null)
            await UploadImage(movieId, MOVIE_FEATURED_BANNER_NAME, images.FeaturedBanner);

        if (images.StandardBanner != null)
            await UploadImage(movieId, MOVIE_STANDARD_BANNER_NAME, images.StandardBanner);
    }

    private string CreateImageFullPath(Guid movieId, string fileName, string contentType)
    {
        var fileNameWithExtension = fileName + "." + contentType.Split('/').Last();
        var movieFullPath = Path.Combine(_hostProvider.GetStorageRootPath(), MOVIE_STORAGE_PATH, movieId.ToString(), fileNameWithExtension);

        return movieFullPath;
    }

    private async Task UploadImage(Guid movieId, string imageName, ImageInfo imageInfo)
    {
        var imageFullPath = CreateImageFullPath(movieId, imageName, imageInfo.ContentType);

        var file = new FileInfo(imageFullPath);

        file.Directory!.Create();

        await using var stream = file.Create();
        await imageInfo.ImageStream.CopyToAsync(stream);
    }
}
