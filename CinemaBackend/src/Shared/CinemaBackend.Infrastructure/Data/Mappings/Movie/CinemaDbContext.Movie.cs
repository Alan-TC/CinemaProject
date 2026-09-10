using CinemaBackend.Domain.Entities.MovieEntities;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data;

internal partial class CinemaDbContext
{
    public DbSet<Movie> Movies { get; set; }
}
