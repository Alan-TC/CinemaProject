using CinemaBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaBackend.Infrastructure.Data.Mappings.TheaterLayout;

public class LayoutGridMapping : IEntityTypeConfiguration<LayoutGrid>
{
    public void Configure(EntityTypeBuilder<LayoutGrid> builder)
    {
        builder.ToTable("layout_grids");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.HasOne<Domain.Entities.TheaterLayout>()
            .WithOne(t => t.Layout)
            .HasForeignKey<LayoutGrid>(x => x.TheaterLayoutId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsMany(x => x.Grid, grid =>
        {
            grid.ToJson();
        });
    }
}
