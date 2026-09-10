using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaBackend.Infrastructure.Data.Mappings.Session;

public class SessionMapping : IEntityTypeConfiguration<Domain.Entities.SessionEntities.Session>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.SessionEntities.Session> builder)
    {
        builder.ToTable("sessions");
        
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .ValueGeneratedNever();

        builder.Property(s => s.Code)
            .IsRequired()
            .HasMaxLength(8);
        
        builder.HasOne(s => s.Movie)
            .WithMany()
            .HasForeignKey(s => s.MovieId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
