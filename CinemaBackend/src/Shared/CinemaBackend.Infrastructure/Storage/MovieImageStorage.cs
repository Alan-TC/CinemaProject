using CinemaBackend.Domain.DTOs.Movie;
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
        var rootPath = MovieImagesRootPath(movieId);

        if (!Directory.Exists(rootPath))
            return new MovieImagesUrlDto(null, null, null);
        
        var files = Directory.EnumerateFiles(rootPath).ToList();

        string? posterUrl = FindAndBuildUrl(files, MOVIE_POSTER_NAME, movieId);
        string? featuredBannerUrl = FindAndBuildUrl(files, MOVIE_FEATURED_BANNER_NAME, movieId);
        string? standardBannerUrl = FindAndBuildUrl(files, MOVIE_STANDARD_BANNER_NAME, movieId);

        return new MovieImagesUrlDto(
            PosterUrl: posterUrl,
            FeaturedBanner: featuredBannerUrl, 
            StandardBanner: standardBannerUrl);
    }

    private string? FindAndBuildUrl(IEnumerable<string> files, string targetName, Guid movieId)
    {
        var file = files.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f).Equals(targetName, StringComparison.OrdinalIgnoreCase));
        if (file == null) return null;

        return $"{_hostProvider.GetRootUrl()}{_hostProvider.GetStorageRequestPath()}/{MOVIE_STORAGE_PATH}/{movieId}/{Path.GetFileName(file)}";
    }

    private string[]? MovieImageFind(Guid movieId, string imageName)
    {
        try
        {
            var rootPath = MovieImagesRootPath(movieId);
            var fileExtensionPattern = ".*";

            string[] paths = Directory.GetFiles(rootPath, imageName + fileExtensionPattern);

            return paths.Length > 0 ? paths : null;
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

    private string MovieImagesRootPath(Guid movieId)
    {
        return Path.Combine(_hostProvider.GetStorageRootPath(), MOVIE_STORAGE_PATH, movieId.ToString());
    }

    private async Task UploadImage(Guid movieId, string imageName, ImageInfo imageInfo)
    {
        var imageFullPath = CreateImageFullPath(movieId, imageName, imageInfo.ContentType);

        var removeImages = MovieImageFind(movieId, imageName);

        if (removeImages != null)
        {
            foreach (var removeImage in removeImages)
            {
                File.Delete(removeImage);
            }
        }

        var file = new FileInfo(imageFullPath);

        file.Directory!.Create();

        await using var stream = file.Create();
        await imageInfo.ImageStream.CopyToAsync(stream);
    }
}
