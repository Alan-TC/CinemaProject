using CinemaBackend.Communication.Requests;

namespace CinemaBackend.Admin.Application.UseCases.AdminUser.Register;

public interface IRegisterUserAdminUseCase
{
    public Task Execute(RegisterUserAdminRequestJson request);
}
