using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;

namespace CinemaBackend.Client.Application.UseCases.Authentication.Login.WithEmailAndPassword;

public interface ILoginWithEmailAndPasswordUseCase
{
    public Task<TokensResponseJson> Execute(LoginRequestJson request);
}
