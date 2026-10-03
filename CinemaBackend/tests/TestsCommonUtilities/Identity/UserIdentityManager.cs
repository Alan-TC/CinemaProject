using CinemaBackend.Domain.Entities.UserEntities;

namespace TestsCommonUtilities.Identity;

public class UserIdentityManager
{
    private readonly User _user;
    private readonly string _password;
    private readonly string _accessToken;
    private readonly string _refreshToken;

    public UserIdentityManager(User user, string password,
        string accessToken, string refreshToken)
    {
        _user = user; 
        _password = password;
        _accessToken = accessToken;
        _refreshToken = refreshToken;
    }

    public Guid GetId() => _user.Id;

    public string GetName() => _user.Name;

    public string GetEmail() => _user.Email;

    public string GetPassword() => _password;

    public string GetAccessToken() => _accessToken;

    public string GetRefreshToken() => _refreshToken;
}
