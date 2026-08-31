using CinemaBackend.Admin.Application.Requests;
using CinemaBackend.Admin.Application.Responses;

namespace CinemaBackend.Admin.Application.UseCases.Authentication.Login.WithEmailAndPassword;

public interface ILoginWithEmailAndPasswordUseCase
{
    public Task<ResponseTokensJson> Execute(LoginRequestJson request);
}
