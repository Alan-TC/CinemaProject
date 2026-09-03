using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.Repositories.AdminUser;
using CinemaBackend.Domain.Security.PasswordHasher;
using CinemaBackend.Domain.Security.Tokens.Admin;

namespace CinemaBackend.Admin.Application.UseCases.Authentication.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCase(
    IPasswordHasher passwordHasher, 
    IAdminUserReadOnlyRepository adminUserReadOnlyRepository,
    IAdminAccessTokenGenerator accessTokenGenerator,
    IAdminRefreshTokenGenerator refreshTokenGenerator
    ) : ILoginWithEmailAndPasswordUseCase
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IAdminUserReadOnlyRepository _adminUserRepository = adminUserReadOnlyRepository;
    private readonly IAdminAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IAdminRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator;

    public async Task<TokensResponseJson> Execute(LoginRequestJson request)
    {
        ValidateAndThrowOnFailures(request);

        var userAdmin = await _adminUserRepository.GetAdminUserByEmail(request.Email);

        if (userAdmin is null)
            throw new InvalidLoginException();

        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, userAdmin.PasswordHash);

        if (isPasswordValid is false)
            throw new InvalidLoginException();

        var accesToken = _accessTokenGenerator.Generate(userAdmin);

        var refreshToken = _refreshTokenGenerator.Generate(userAdmin);

        return new TokensResponseJson()
        {
            AccessToken = accesToken,
            RefreshToken = refreshToken
        };
    }

    private static void ValidateAndThrowOnFailures(LoginRequestJson request)
    {
        var validator = new LoginWithEmailAndPasswordValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
            throw new InvalidLoginException();
    }
}
