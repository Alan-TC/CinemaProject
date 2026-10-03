using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.Repositories.User;
using CinemaBackend.Domain.Security.Tokens.User;

namespace CinemaBackend.Client.Application.UseCases.Authentication.RefreshToken;

public class RefreshTokenUseCase(
    IUserReadOnlyRepository userReadOnlyRepository,
    IUserAccessTokenGenerator userAccessTokenGenerator,
    IUserRefreshTokenService userRefreshTokenService
    ) : IRefreshTokenUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;
    private readonly IUserAccessTokenGenerator _userAccessTokenGenerator = userAccessTokenGenerator;
    private readonly IUserRefreshTokenService _userRefreshTokenService = userRefreshTokenService;

    public async Task<TokensResponseJson> Execute(RefreshTokenRequestJson request)
    {
        var result = await _userRefreshTokenService.Validate(request.RefreshToken);

        if (result == false)
            throw new InvalidRefreshTokenException();

        var userId = _userRefreshTokenService.GetGuid(request.RefreshToken);

        if (userId == null)
            throw new InvalidRefreshTokenException();

        var user = await _userReadOnlyRepository.GetUserById((Guid)userId);
        
        if (user == null)
            throw new InvalidRefreshTokenException();

        var accessToken = _userAccessTokenGenerator.Generate(user);

        return new TokensResponseJson
        {
            AccessToken = accessToken,
            RefreshToken = request.RefreshToken
        };
    }
}
