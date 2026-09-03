using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;

namespace CinemaBackend.Admin.Application.UseCases.Authentication.RefreshToken;

public interface IRefreshTokenUseCase
{
    Task<TokensResponseJson> Execute(RefreshTokenRequestJson request);
}
