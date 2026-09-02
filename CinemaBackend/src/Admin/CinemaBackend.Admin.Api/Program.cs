using Scalar.AspNetCore;
using CinemaBackend.Infrastructure;
using CinemaBackend.Admin.Application;
using CinemaBackend.Admin.Api.WebApplicationExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddAdminInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

await app.Services.ApplyMigrations();

await app.RegisterInitialAdminUser();

app.Run();