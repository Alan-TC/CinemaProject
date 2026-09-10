using CinemaBackend.Domain.Entities.SessionEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaBackend.Infrastructure.Data.Mappings.Session;

public class SessionRoomSeatMapping : IEntityTypeConfiguration<SessionRoomSeat>
{
    public void Configure(EntityTypeBuilder<SessionRoomSeat> builder)
    {
        builder.ToTable("session_room_seats");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id)
            .ValueGeneratedNever();
        
        builder.Property(s => s.Status)
            .HasConversion<string>();

        builder.HasOne<SessionRoom>()
            .WithMany(r => r.Seats)
            .HasForeignKey(s => s.SessionRoomId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
