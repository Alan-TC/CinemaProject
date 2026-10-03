using CinemaBackend.Communication.Requests;

namespace CinemaBackend.Client.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    Task Execute(UserRequestJson request);
}
