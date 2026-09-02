using CinemaBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data;
internal partial class CinemaDbContext
{
    public DbSet<AdminUser> AdminUsers { get; set; } = null!;
}