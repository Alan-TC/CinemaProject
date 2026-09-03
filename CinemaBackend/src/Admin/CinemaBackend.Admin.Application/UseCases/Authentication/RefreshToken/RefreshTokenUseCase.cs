using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.Repositories.AdminUser;
using CinemaBackend.Domain.Security.Tokens.Admin;

namespace CinemaBackend.Admin.Application.UseCases.Authentication.RefreshToken;

public class RefreshTokenUseCase(
    IAdminUserReadOnlyRepository adminUserReadOnlyRepository,
    IAdminAccessTokenGenerator adminAccessTokenGenerator,
    IAdminRefreshTokenService adminRefreshTokenService
    ) : IRefreshTokenUseCase
{
    private readonly IAdminUserReadOnlyRepository _adminUserReadOnlyRepository = adminUserReadOnlyRepository;
    private readonly IAdminAccessTokenGenerator _adminAccessTokenGenerator = adminAccessTokenGenerator;
    private readonly IAdminRefreshTokenService _adminRefreshTokenService = adminRefreshTokenService;

    public async Task<TokensResponseJson> Execute(RefreshTokenRequestJson request)
    {
        var result = await _adminRefreshTokenService.Validate(request.RefreshToken);

        if (result == false)
            throw new InvalidRefreshTokenException();

        var adminUserId = _adminRefreshTokenService.GetGuid(request.RefreshToken);

        if (adminUserId == null)
            throw new InvalidRefreshTokenException();

        var adminUser = await _adminUserReadOnlyRepository.GetAdminUserById((Guid)adminUserId);
        
        if (adminUser == null)
            throw new InvalidRefreshTokenException();

        var accessToken = _adminAccessTokenGenerator.Generate(adminUser);

        return new TokensResponseJson
        {
            AccessToken = accessToken,
            RefreshToken = request.RefreshToken
        };
    }
}
