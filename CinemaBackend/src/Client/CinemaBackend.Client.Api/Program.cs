using CinemaBackend.Client.Api.Exceptions;
using CinemaBackend.Client.Api.Extensions;
using CinemaBackend.Client.Api.Tokens;
using CinemaBackend.Client.Application;
using CinemaBackend.Infrastructure;
using CinemaBackend.Infrastructure.Security.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Scalar.AspNetCore;
using System.Text.Json;
using System.Text.Json.Serialization;

void configureJson(JsonSerializerOptions options)
{
    options.Converters.Add(new JsonStringEnumConverter());
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddOpenApiWithJwtBearer();

builder.Services.AddControllers()
    .AddJsonOptions(options => configureJson(options.JsonSerializerOptions));

// Para o OpenApi pegar as conversões. Ele se baseia na configuração das MinimalAPIs
builder.Services.Configure<JsonOptions>(options =>
    configureJson(options.SerializerOptions));

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddApiServices(builder);
builder.Services.AddApplication();
builder.Services.AddClientInfrastructure(builder.Configuration);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwtOption =>
    {
        jwtOption.TokenValidationParameters = TokenValidationUtils.GetParameters(builder.Configuration);

        jwtOption.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var tokenUse = context.Principal?.FindFirst(TokensTypes.TokenTypeClaimName)?.Value;

                if (tokenUse != TokensTypes.Access)
                {
                    context.Fail("Invalid access token");
                }

                return Task.CompletedTask;
            },
            OnChallenge = async context =>
            {
                await TokenValidationUtils.OnChallenge(context);
            }
        };
    });

var app = builder.Build();

app.UseExceptionHandler(_ => { });

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.Services.ApplyMigrations();

app.Run();