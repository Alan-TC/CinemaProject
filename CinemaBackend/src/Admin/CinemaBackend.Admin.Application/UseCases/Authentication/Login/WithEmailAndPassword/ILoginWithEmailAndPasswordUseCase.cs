using CinemaBackend.SharedKernel.Requests;
using CinemaBackend.SharedKernel.Responses;

namespace CinemaBackend.Admin.Application.UseCases.Authentication.Login.WithEmailAndPassword;

public interface ILoginWithEmailAndPasswordUseCase
{
    public Task<ResponseTokensJson> Execute(LoginRequestJson request);
}
