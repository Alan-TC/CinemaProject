using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data;

internal partial class CinemaDbContext
{
    public DbSet<Domain.Entities.TheaterLayout> TheaterLayouts { get; set; }
}
