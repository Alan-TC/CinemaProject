namespace CinemaBackend.SharedKernel.Requests;

public class RegisterUserAdminRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;    
}
