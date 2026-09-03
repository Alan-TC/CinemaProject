using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;

namespace CinemaBackend.Admin.Application.UseCases.Authentication.Login.WithEmailAndPassword;

public interface ILoginWithEmailAndPasswordUseCase
{
    public Task<TokensResponseJson> Execute(LoginRequestJson request);
}
