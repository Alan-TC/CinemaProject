using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaBackend.Infrastructure.Data.Mappings.AdminUser;

public class AdminUserMapping : IEntityTypeConfiguration<Domain.Entities.AdminUserEntities.AdminUser>
{
    public void Configure(EntityTypeBuilder<Domain.Entities.AdminUserEntities.AdminUser> builder)
    {
        builder.ToTable("admin_users");
        
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedNever();

        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(256);
    }
}