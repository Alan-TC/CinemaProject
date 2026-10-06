using CinemaBackend.Domain.Entities.PaymentEntities;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data;

internal partial class CinemaDbContext
{
    public DbSet<Payment> Payments { get; set; }
}
