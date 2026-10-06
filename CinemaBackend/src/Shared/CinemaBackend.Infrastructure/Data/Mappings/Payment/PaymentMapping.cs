using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaBackend.Infrastructure.Data.Mappings.Payment;

public class PaymentMapping : IEntityTypeConfiguration<Domain.Entities.PaymentEntities.Payment>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.PaymentEntities.Payment> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasOne(x => x.Session)
            .WithOne()
            .HasForeignKey<Domain.Entities.PaymentEntities.Payment>(x => x.SessionId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(x => x.Status)
            .HasConversion<string>();
    }
}