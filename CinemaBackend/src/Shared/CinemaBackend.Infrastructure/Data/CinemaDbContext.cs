using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data;

internal class CinemaDbContext(DbContextOptions options) : DbContext(options)
{
        
}
