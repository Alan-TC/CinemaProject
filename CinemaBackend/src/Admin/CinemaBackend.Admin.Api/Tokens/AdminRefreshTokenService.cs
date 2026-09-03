using CinemaBackend.Domain.Security.Tokens.Admin;
using CinemaBackend.Infrastructure.Security.Tokens;
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

        if (result.IsValid == false)
            return false;
        
        if (result.Claims.TryGetValue(TokensTypes.TokenTypeClaimName, out object? value) &&
            (string)value == TokensTypes.Refresh)
            return true;

        return false;
    }
}
