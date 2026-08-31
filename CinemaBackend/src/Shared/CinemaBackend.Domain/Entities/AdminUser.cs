namespace CinemaBackend.Domain.Entities;

public class AdminUser : EntityBase
{
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}
