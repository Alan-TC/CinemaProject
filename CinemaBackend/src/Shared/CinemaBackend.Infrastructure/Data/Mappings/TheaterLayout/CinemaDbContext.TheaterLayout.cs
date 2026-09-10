using CinemaBackend.Domain.Entities.TheaterLayoutEntities;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data;

internal partial class CinemaDbContext
{
    public DbSet<TheaterLayout> TheaterLayouts { get; set; }
}
