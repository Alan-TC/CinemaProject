using CinemaBackend.Domain.Entities.SessionEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaBackend.Infrastructure.Data.Mappings.Session;

public class SessionRoomMapping : IEntityTypeConfiguration<SessionRoom>
{
    public void Configure(EntityTypeBuilder<SessionRoom> builder)
    {
        builder.ToTable("session_rooms");

        builder.HasKey(sr => sr.Id);
        builder.Property(sr => sr.Id)
            .ValueGeneratedNever();
        
        builder.HasOne<Domain.Entities.SessionEntities.Session>()
            .WithOne(s => s.SessionRoom)
            .HasForeignKey<SessionRoom>(sr => sr.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
