using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.Communication.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CinemaBackend.Admin.Api.Tokens;

public static class TokenValidationUtils
{
    public static TokenValidationParameters GetParameters(IConfiguration configuration)
    {
        var signingKey = configuration["Jwt:SigningKey"];
        var audience = configuration["Jwt:Audience"];
        var issuer = configuration["Jwt:Issuer"];

        return new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey!)),
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    }
    
    public static async Task OnChallenge(JwtBearerChallengeContext context)
    {
        context.HandleResponse();

        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";

        var response = context.AuthenticateFailure switch
        {
            null => new ErrorResponseJson(ResourceErrorMessages.VALIDATION_ACCESS_TOKEN_REQUIRED),
            SecurityTokenExpiredException => new ErrorResponseJson(ResourceErrorMessages.VALIDATION_ACCESS_TOKEN_EXPIRED, true),
            _ => new ErrorResponseJson(ResourceErrorMessages.VALIDATION_RESOURCE_ACCESS_DENIED)
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}
