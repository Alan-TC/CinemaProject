using CinemaBackend.Domain.Entities;

namespace TestsCommonUtilities.Identity;

public class AdminUserIdentityManager
{
    private readonly AdminUser _adminUser;
    private readonly string _password;
    private readonly string _accessToken;
    private readonly string _refreshToken;

    public AdminUserIdentityManager(AdminUser adminUser, string password, 
        string accessToken, string refreshToken)
    {
        _adminUser = adminUser; 
        _password = password;
        _accessToken = accessToken;
        _refreshToken = refreshToken;
    }

    public Guid GetId() => _adminUser.Id;
    
    public string GetEmail() => _adminUser.Email;

    public string GetPassword() => _password;

    public string GetAccessToken() => _accessToken;

    public string GetRefreshToken() => _refreshToken;
}
