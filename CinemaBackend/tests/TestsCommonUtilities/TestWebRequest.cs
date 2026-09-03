using System.Net.Http.Json;

namespace TestsCommonUtilities;

public class TestWebRequest<TEntryPoint> where TEntryPoint : class
{
    private readonly HttpClient _httpClient;

    public TestWebRequest(CommonWebApplicationFactory<TEntryPoint> factory)
    {
        _httpClient = factory.CreateClient();
    }

    public async Task<HttpResponseMessage> Post(string requestUri, object request)
    {
        return await _httpClient.PostAsJsonAsync(requestUri, request);
    }
}
