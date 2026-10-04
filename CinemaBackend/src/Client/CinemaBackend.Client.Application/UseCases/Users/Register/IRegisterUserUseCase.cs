using CinemaBackend.Communication.Requests;

namespace CinemaBackend.Client.Application.UseCases.Users.Register;

public interface IRegisterUserUseCase
{
    Task Execute(UserRequestJson request);
}
