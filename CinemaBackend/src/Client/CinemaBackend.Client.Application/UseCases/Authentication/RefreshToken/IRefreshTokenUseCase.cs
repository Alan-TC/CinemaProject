using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;

namespace CinemaBackend.Client.Application.UseCases.Authentication.RefreshToken;

public interface IRefreshTokenUseCase
{
    Task<TokensResponseJson> Execute(RefreshTokenRequestJson request);
}
