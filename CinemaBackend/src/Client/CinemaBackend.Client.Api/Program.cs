using CinemaBackend.Admin.Api.Exceptions;
using CinemaBackend.Client.Application;
using CinemaBackend.Infrastructure;
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

builder.Services.AddControllers()
    .AddJsonOptions(options => configureJson(options.JsonSerializerOptions));

// Para o OpenApi pegar as conversões. Ele se baseia na configuração das MinimalAPIs
builder.Services.Configure<JsonOptions>(options =>
    configureJson(options.SerializerOptions));

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddApplication();
builder.Services.AddClientInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(_ => { });

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


app.UseHttpsRedirection();

app.MapControllers();

await app.Services.ApplyMigrations();

app.Run();