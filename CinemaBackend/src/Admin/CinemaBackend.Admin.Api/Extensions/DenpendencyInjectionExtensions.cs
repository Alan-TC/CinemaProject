using CinemaBackend.Admin.Api.Tokens;
using CinemaBackend.Domain.Security.Tokens.Admin;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;

namespace CinemaBackend.Admin.Api.Extensions;

public static class DenpendencyInjectionExtensions
{
    public static void AddOpenApiWithJwtBearer(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                var bearerScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                };

                document.Components ??= new OpenApiComponents();
                document.AddComponent("Bearer", bearerScheme);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                };

                document.Security ??= new List<OpenApiSecurityRequirement>();
                document.Security.Add(securityRequirement);

                return Task.CompletedTask;
            });
        });
    }

    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAdminRefreshTokenService>(_ =>
        {
            return new AdminRefreshTokenService(configuration);
        });
    }
}
