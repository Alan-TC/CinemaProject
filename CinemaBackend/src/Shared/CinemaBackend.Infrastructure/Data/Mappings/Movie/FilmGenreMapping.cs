using CinemaBackend.Domain.Entities.MovieEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaBackend.Infrastructure.Data.Mappings.Movie;

public class FilmGenreMapping : IEntityTypeConfiguration<FilmGenre>
{
    public void Configure(EntityTypeBuilder<FilmGenre> builder)
    {
        builder.ToTable("film_genres");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Genre)
            .HasConversion<string>();

        builder.HasOne<Domain.Entities.MovieEntities.Movie>()
            .WithMany(m => m.FilmGenres)
            .HasForeignKey(x => x.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
