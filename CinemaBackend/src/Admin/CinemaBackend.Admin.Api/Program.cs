using Scalar.AspNetCore;
using CinemaBackend.Infrastructure;
using CinemaBackend.Admin.Application;
using CinemaBackend.Admin.Api.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CinemaBackend.Admin.Api.Tokens;
using CinemaBackend.Admin.Api.Extensions.WebApplicationExtensions;
using CinemaBackend.Admin.Api.Extensions;
using CinemaBackend.Infrastructure.Security.Tokens;
using CinemaBackend.Communication.ResourceFiles.Errors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddOpenApiWithJwtBearer();

builder.Services.AddApiServices(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddAdminInfrastructure(builder.Configuration);

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

app.UseExceptionHandler(_ => {});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.Services.ApplyMigrations();

await app.RegisterInitialAdminUser();

app.Run();