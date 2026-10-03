using CinemaBackend.Infrastructure.Storage;

namespace CinemaBackend.Client.Api.HostProvider;

public class HostInfoProvider(string rootUrl, string storageRootPath, string storageRequestPath) : IHostInfoProvider
{
    private readonly string _rootUrl = rootUrl.Last() != '/' ? rootUrl : rootUrl[..(rootUrl.Length - 1)];
    private readonly string _storagePath = storageRootPath;
    private readonly string _storageRequestPath = storageRequestPath.First() == '/' ? storageRequestPath : "/" + storageRequestPath;

    public string GetRootUrl() => _rootUrl;

    public string GetStorageRequestPath() => _storageRequestPath;

    public string GetStorageRootPath() => _storagePath;
}
