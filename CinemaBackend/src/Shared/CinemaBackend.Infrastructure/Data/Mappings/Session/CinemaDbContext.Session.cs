using CinemaBackend.Domain.Entities.SessionEntities;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data;

internal partial class CinemaDbContext
{
    public DbSet<Session> Sessions { get; set; }
}
