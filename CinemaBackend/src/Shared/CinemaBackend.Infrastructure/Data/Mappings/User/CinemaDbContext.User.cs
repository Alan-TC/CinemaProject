using CinemaBackend.Domain.Entities.UserEntities;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data;

internal partial class CinemaDbContext
{
    public DbSet<User> Users { get; set; }
}
