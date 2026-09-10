using CinemaBackend.Domain.Entities.SessionEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaBackend.Infrastructure.Data.Mappings.Session;

public class SessionPricingMapping : IEntityTypeConfiguration<SessionPricing>
{
    public void Configure(EntityTypeBuilder<SessionPricing> builder)
    {
        builder.ToTable("session_pricings");

        builder.HasKey(x => x.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.HasOne<Domain.Entities.SessionEntities.Session>()
            .WithOne(s => s.Pricing)
            .HasForeignKey<SessionPricing>(p => p.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
