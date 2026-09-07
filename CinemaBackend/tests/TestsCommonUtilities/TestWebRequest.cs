using System.Net.Http.Headers;
using System.Net.Http.Json;
using CinemaBackend.Infrastructure.Data;
using CinemaBackend.SharedKernel.Extensions;
using Microsoft.Extensions.DependencyInjection;
using TestsCommonUtilities.Extensions;

namespace TestsCommonUtilities;

internal class TestWebRequest<TEntryPoint> where TEntryPoint : class
{
    private readonly HttpClient _httpClient;
    public readonly CinemaDbContext DbContext;

    public TestWebRequest(CommonWebApplicationFactory<TEntryPoint> factory)
    {
        _httpClient = factory.CreateClient();

        var scope = factory.Services.CreateScope();

        DbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();
    }

    public async Task<HttpResponseMessage> Post(string requestUri, object request, string? token = null)
    {
        AddAccessToken(token);

        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }

    public async Task<HttpResponseMessage> PostMultipartForm(string requestUri, object request, string? token = null)
    {
        AddAccessToken(token);

        var multipartRequest = MultipartFormDataSerializer.Serialize(request);

        return await _httpClient.PostAsync(requestUri, multipartRequest);
    }

    public async Task<HttpResponseMessage> Put(string requestUri, object request, string? token = null)
    {
        AddAccessToken(token);

        return await _httpClient.PutAsJsonAsync(requestUri, request);
    }

    private void AddAccessToken(string? token)
    {
        if (token.IsNotEmpty())
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
