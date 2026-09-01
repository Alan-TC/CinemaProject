using CinemaBackend.Communication.Requests;

namespace CinemaBackend.Admin.Application.UseCases.Authentication.Register;

public interface IRegisterUserAdminUseCase
{
    public void Execute(RegisterUserAdminRequest request);
}
