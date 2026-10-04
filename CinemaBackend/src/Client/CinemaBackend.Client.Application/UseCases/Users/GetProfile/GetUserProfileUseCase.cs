using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.Identity.Client;

namespace CinemaBackend.Client.Application.UseCases.Users.GetProfile;

public class GetUserProfileUseCase(ILoggedUser loggedUser) : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<UserProfileJson> Execute()
    {
        var user = await _loggedUser.Get();

        return new UserProfileJson
        {
            Name = user.Name,
            Email = user.Email
        };
    }
}
