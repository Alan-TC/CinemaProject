using CinemaBackend.Admin.Api.HostProvider;
using CinemaBackend.Admin.Api.Tokens;
using CinemaBackend.Domain.Security.Tokens.Admin;
using CinemaBackend.Infrastructure.Storage;
<<<<<<< HEAD
=======
using CinemaBackend.SharedKernel.Extensions;
>>>>>>> register-movies
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

    public static void AddApiServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        services.AddScoped<IAdminRefreshTokenService>(_ =>
        {
            return new AdminRefreshTokenService(configuration);
        });

        services.AddScoped<IHostInfoProvider>(_ =>
        {
            var baseUrl = configuration["Host:BaseUrl"]!;

            var storageRoot = builder.Environment.IsDevelopment()
<<<<<<< HEAD
                ? PathOfDevUtils(builder.Environment.ContentRootPath) 
=======
                ? PathOfDevUtils(configuration, builder.Environment.ContentRootPath) 
>>>>>>> register-movies
                : configuration["Host:StorageRootPath"]!;

            var requestPath = configuration["Host:StorageRequestPath"]!;

            return new HostInfoProvider(baseUrl, storageRoot, requestPath);
        });
    }
<<<<<<< HEAD

    private static string PathOfDevUtils(string contentRootPath)
    {
=======
    private static string PathOfDevUtils(IConfiguration configuration, string contentRootPath)
    {
        string? path = configuration["Host:StorageRootPath"];

        if (path.IsNotEmpty())
            return path;

>>>>>>> register-movies
        var adminPath = Directory.GetParent(contentRootPath)!.FullName;
        var srcPath = Directory.GetParent(adminPath)!.FullName;
        var slnxPath = Directory.GetParent(srcPath)!.FullName;

        return Path.Combine(slnxPath, "dev-storage");
    }
}
