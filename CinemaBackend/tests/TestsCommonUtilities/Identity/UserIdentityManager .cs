using CinemaBackend.Domain.Entities.UserEntities;

namespace TestsCommonUtilities.Identity;

public class UserIdentityManager
{
    private readonly User _user;
    private readonly string _password;

    public UserIdentityManager(User user, string password)
    {
        _user = user; 
        _password = password;
    }

    public Guid GetId() => _user.Id;

    public string GetName() => _user.Name;

    public string GetEmail() => _user.Email;

    public string GetPassword() => _password;
}
