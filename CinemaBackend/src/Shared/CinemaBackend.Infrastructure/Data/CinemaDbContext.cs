using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data;

internal partial class CinemaDbContext(DbContextOptions<CinemaDbContext> options) : DbContext(options)
{
    override protected void OnModelCreating(ModelBuilder modelBuilder) => 
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CinemaDbContext).Assembly);
}
