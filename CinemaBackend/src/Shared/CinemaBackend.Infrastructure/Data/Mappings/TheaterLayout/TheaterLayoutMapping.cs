using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaBackend.Infrastructure.Data.Mappings.TheaterLayout;

public class TheaterLayoutMapping : IEntityTypeConfiguration<Domain.Entities.TheaterLayoutEntities.TheaterLayout>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.TheaterLayoutEntities.TheaterLayout> builder)
    {
        builder.ToTable("theater_layouts");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        builder.Property(x => x.Tag)
            .HasMaxLength(8);
    }
}