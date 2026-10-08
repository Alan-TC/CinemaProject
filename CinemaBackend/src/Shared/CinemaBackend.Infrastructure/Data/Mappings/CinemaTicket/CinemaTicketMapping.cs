using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaBackend.Infrastructure.Data.Mappings.Payment;

public class CinemaTicketMapping : IEntityTypeConfiguration<Domain.Entities.CinemaTicketEntities.CinemaTicket>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.CinemaTicketEntities.CinemaTicket> builder)
    {
        builder.ToTable("cinema_tickets");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasOne(x => x.Payment)
            .WithOne()
            .HasForeignKey<Domain.Entities.CinemaTicketEntities.CinemaTicket>(x => x.PaymentId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}