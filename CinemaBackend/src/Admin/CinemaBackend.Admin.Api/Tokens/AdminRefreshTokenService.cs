using CinemaBackend.Domain.Security.Tokens.Admin;
using Microsoft.IdentityModel.JsonWebTokens;

namespace CinemaBackend.Admin.Api.Tokens;

public class AdminRefreshTokenService(IConfiguration configuration) : IAdminRefreshTokenService
{
    private readonly IConfiguration _configuration = configuration;
    public Guid? GetGuid(string refreshToken)
    {
        var handler = new JsonWebTokenHandler();

        var jsonWebToken = handler.ReadJsonWebToken(refreshToken);

        var subject = jsonWebToken.Subject;

        return Guid.Parse(subject);
    }

    public async Task<bool> Validate(string refreshToken)
    {
        var handler = new JsonWebTokenHandler();

        var validationParameters = TokenValidationUtils.GetParameters(_configuration);

        var result = await handler.ValidateTokenAsync(refreshToken, validationParameters);

        return result.IsValid;
    }
}
