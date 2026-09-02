using System.Security.Claims;
using CinemaBackend.Domain.Security.Tokens;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace CinemaBackend.Infrastructure.Security.Tokens;

public class JwtTokenHandler : IAccessTokenGenerator, IRefreshTokenGenerator
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

    string IAccessTokenGenerator.Generate(string subClaim)
    {
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, subClaim)
        };

        var token = CreateToken(claims, DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes));

        return token;
    }

    string IRefreshTokenGenerator.Generate(string subClaim)
    {
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, subClaim)
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
