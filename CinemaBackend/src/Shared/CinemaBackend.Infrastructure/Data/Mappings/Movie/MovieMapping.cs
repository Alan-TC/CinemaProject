using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaBackend.Infrastructure.Data.Mappings.Movie;

public class MovieMapping : IEntityTypeConfiguration<Domain.Entities.Movie>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.Movie> builder)
    {
        builder.ToTable("movies");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(256);
        
        builder.Property(x => x.Sinopse)
            .IsRequired()
            .HasMaxLength(2048);
        
        builder.Property(x => x.Duration)
            .IsRequired();
        
        builder.Property(x => x.ClassInd)
            .HasConversion<string>();
    }
}
