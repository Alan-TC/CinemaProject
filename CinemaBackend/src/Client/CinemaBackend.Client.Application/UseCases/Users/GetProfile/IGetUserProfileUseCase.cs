using CinemaBackend.Communication.Responses;

namespace CinemaBackend.Client.Application.UseCases.Users.GetProfile;

public interface IGetUserProfileUseCase
{
    Task<UserProfileJson> Execute();
}
