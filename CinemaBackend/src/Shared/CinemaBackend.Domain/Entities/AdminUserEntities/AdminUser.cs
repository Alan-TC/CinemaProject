namespace CinemaBackend.Domain.Entities.AdminUserEntities;

public class AdminUser : EntityBase
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}
