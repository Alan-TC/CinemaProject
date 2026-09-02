using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.Repositories.AdminUser;
using CinemaBackend.Domain.Security.Tokens;
using CinemaBackend.Domain.Security.PasswordHasher;

namespace CinemaBackend.Admin.Application.UseCases.Authentication.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCase(
    IPasswordHasher passwordHasher, 
    IAdminUserReadOnlyRepository adminUserReadOnlyRepository,
    IAccessTokenGenerator accessTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator
    ) : ILoginWithEmailAndPasswordUseCase
{
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IAdminUserReadOnlyRepository _adminUserRepository = adminUserReadOnlyRepository;
    private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator = refreshTokenGenerator;

    public async Task<ResponseTokensJson> Execute(LoginRequestJson request)
    {
        ValidateAndThrowOnFailures(request);

        var user = await _adminUserRepository.GetAdminUserByEmail(request.Email);

        if (user is null)
            throw new InvalidLoginException();

        var isPasswordValid = _passwordHasher.VerifyPassword(request.Password, user.PasswordHash);

        if (isPasswordValid is false)
            throw new InvalidLoginException();

        var accesToken = _accessTokenGenerator.Generate(user.Id.ToString());

        var refreshToken = _refreshTokenGenerator.Generate(user.Id.ToString());

        return new ResponseTokensJson()
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
