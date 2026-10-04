using CinemaBackend.Domain.Security.Tokens.User;

namespace CinemaBackend.Client.Api.Tokens;

public class UserAccessTokenProvider(IHttpContextAccessor httpContextAccessor) : IUserAccessTokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string GetToken()
    {
        var accessToken = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return accessToken["Bearer ".Length..];
    }
}
