namespace CinemaBackend.Infrastructure.Storage;

public interface IHostInfoProvider
{
    string GetRootUrl();
    string GetStorageRootPath();
    string GetStorageRequestPath();
}
