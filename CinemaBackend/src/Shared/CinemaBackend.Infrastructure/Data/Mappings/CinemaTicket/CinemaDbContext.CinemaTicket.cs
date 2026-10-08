using CinemaBackend.Domain.Entities.CinemaTicketEntities;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data;

internal partial class CinemaDbContext
{
    public DbSet<CinemaTicket> CinemaTickets { get; set; }
}
