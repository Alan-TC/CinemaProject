using CinemaBackend.Domain.Entities.MovieEntities;
using CinemaBackend.Domain.Entities.TheaterLayoutEntities;
using CinemaBackend.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using TestsCommonUtilities.Entities;

namespace TestsCommonUtilities;

public abstract class CommonWebApplicationFactory<TEntryPoint> : 
    WebApplicationFactory<TEntryPoint>, IAsyncLifetime where TEntryPoint : class
{
    public Movie Movie { get; private set; } = default!;
    public Movie Movie2 { get; private set; } = default!;
    public TheaterLayout TheaterLayout { get; private set; } = default!;

    private readonly PostgreSqlContainer _postgreSqlContainer;
    private readonly string _storagePath;

    public CommonWebApplicationFactory()
    {
        _postgreSqlContainer = new PostgreSqlBuilder("postgres:17")
            .WithDatabase("cinema_development_db")
            .Build();
        
        _storagePath = Path.Combine(Environment.CurrentDirectory, Guid.CreateVersion7().ToString());
    }

    public async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        Directory.CreateDirectory(_storagePath);

        await InitializeDatabase();

        await using var scope = Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<CinemaDbContext>();

        Movie = await CreateMovie(dbContext);
        Movie2 = await CreateMovie(dbContext);
        TheaterLayout = await CreateTheaterLayout(dbContext);
    }

    private static async Task<TheaterLayout> CreateTheaterLayout(CinemaDbContext dbContext)
    {
        var theaterLayout = new TheaterLayoutBuilder().Build();

        await dbContext.TheaterLayouts.AddAsync(theaterLayout);
        await dbContext.SaveChangesAsync();

        return theaterLayout;
    }

    private static async Task<Movie> CreateMovie(CinemaDbContext dbContext)
    {
        var movie = new MovieBuilder().Build();

        await dbContext.Movies.AddAsync(movie);
        await dbContext.SaveChangesAsync();

        return movie;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseConfiguration(GetConfiguration())
            .ConfigureAppConfiguration((_, configuration) =>
            {
                var parameters = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DbConnection"] = _postgreSqlContainer.GetConnectionString(),
                    ["Host:StorageRootPath"] = _storagePath,
                    ["Host:StorageRequestPath"] = "assets" 
                };

                configuration.AddInMemoryCollection(parameters);
            });
    }

    protected abstract Task InitializeDatabase();

    protected abstract IConfiguration GetConfiguration();

    Task IAsyncLifetime.DisposeAsync() {
        Directory.Delete(_storagePath, true);

        return _postgreSqlContainer.StopAsync();
    }
}
