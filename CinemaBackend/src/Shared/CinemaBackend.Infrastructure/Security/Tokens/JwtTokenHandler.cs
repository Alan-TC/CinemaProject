using System.Security.Claims;
using CinemaBackend.Domain.Entities.AdminUserEntities;
using CinemaBackend.Domain.Security.Tokens.Admin;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CinemaBackend.Infrastructure.Security.Tokens;

public class JwtTokenHandler : IAdminAccessTokenGenerator, IAdminRefreshTokenGenerator
{
    private readonly string _signingtKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly uint _accessTokenExpirationMinutes;
    private readonly uint _refreshTokenExpirationMinutes;

    public JwtTokenHandler(
        string secretKey, 
        string issuer, 
        string audience, 
        uint accessTokenExpirationMinutes, 
        uint refreshTokenExpirationMinutes)
    {
        _signingtKey = secretKey;
        _issuer = issuer;
        _audience = audience;
        _accessTokenExpirationMinutes = accessTokenExpirationMinutes;
        _refreshTokenExpirationMinutes = refreshTokenExpirationMinutes;
    }

    string IAdminAccessTokenGenerator.Generate(AdminUser adminUser)
    {
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, adminUser.Id.ToString()),
            new (TokensTypes.TokenTypeClaimName, TokensTypes.Access)
        };

        var token = CreateToken(claims, DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes));

        return token;
    }

    string IAdminRefreshTokenGenerator.Generate(AdminUser adminUse)
    {
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, adminUse.Id.ToString()),
            new (TokensTypes.TokenTypeClaimName, TokensTypes.Refresh)
        };

        var token = CreateToken(claims, DateTime.UtcNow.AddMinutes(_refreshTokenExpirationMinutes));

        return token;
    }

    private string CreateToken(List<Claim> claims, DateTime expires)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(SigningKey(), SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JsonWebTokenHandler();

        return tokenHandler.CreateToken(tokenDescriptor);
    }

    private SymmetricSecurityKey SigningKey()
    {
        var keyBytes = System.Text.Encoding.UTF8.GetBytes(_signingtKey);

        return new SymmetricSecurityKey(keyBytes);
    }
}
