using CinemaBackend.Domain.Entities.PaymentEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaBackend.Infrastructure.Data.Mappings.TheaterLayout;

public class AllocatedSeatMapping : IEntityTypeConfiguration<AllocatedSeat>
{
    public void Configure(EntityTypeBuilder<AllocatedSeat> builder)
    {
        builder.ToTable("allocated_seats");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasOne<Domain.Entities.PaymentEntities.Payment>()
            .WithMany(x => x.Seats)
            .HasForeignKey(s => s.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
