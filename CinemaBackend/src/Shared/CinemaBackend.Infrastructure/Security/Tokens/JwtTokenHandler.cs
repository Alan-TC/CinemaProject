using System.Security.Claims;
using CinemaBackend.Domain.Entities.AdminUserEntities;
using CinemaBackend.Domain.Entities.UserEntities;
using CinemaBackend.Domain.Security.Tokens.Admin;
using CinemaBackend.Domain.Security.Tokens.User;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CinemaBackend.Infrastructure.Security.Tokens;

public class JwtTokenHandler 
    : IAdminAccessTokenGenerator, IAdminRefreshTokenGenerator, IUserAccessTokenGenerator, IUserRefreshTokenGenerator
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

    string IAdminRefreshTokenGenerator.Generate(AdminUser adminUser)
    {
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, adminUser.Id.ToString()),
            new (TokensTypes.TokenTypeClaimName, TokensTypes.Refresh)
        };

        var token = CreateToken(claims, DateTime.UtcNow.AddMinutes(_refreshTokenExpirationMinutes));

        return token;
    }

    string IUserAccessTokenGenerator.Generate(User user)
    {
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Name, user.Name),
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (TokensTypes.TokenTypeClaimName, TokensTypes.Access)
        };

        var token = CreateToken(claims, DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes));

        return token;
    }

    string IUserRefreshTokenGenerator.Generate(User user)
    {
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
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
