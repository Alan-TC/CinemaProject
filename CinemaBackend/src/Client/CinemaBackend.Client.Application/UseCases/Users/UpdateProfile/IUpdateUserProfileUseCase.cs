using CinemaBackend.Communication.Requests;

namespace CinemaBackend.Client.Application.UseCases.Users.UpdateProfile;

public interface IUpdateUserProfileUseCase
{
    Task Execute(UserProfileRequestJson request);
}
