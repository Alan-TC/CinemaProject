using System.Net;
using CinemaBackend.Client.Application.UseCases.Authentication.Login.WithEmailAndPassword;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.ResourceFiles.Errors;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Entities;
using TestsCommonUtilities.Repositories.User;
using TestsCommonUtilities.Requests;
using TestsCommonUtilities.Security;
using Xunit.Abstractions;

namespace Client.UseCases.Tests.Authentication.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCaseTests(ITestOutputHelper output)
{
    private readonly TestLoggingClass _testLogger = new(output);

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        var (user, password) = new UserBuilder()
            .Build();
        context.Add(user);

        var request = new LoginRequestJsonBuilder()
            .WithEmail(user.Email)
            .WithPassword(password)
            .Build();
        context.Add(context);

        var useCase = CreateUseCase(user, request.Password);

        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var result = await useCase.Execute(request);

            result.AccessToken.ShouldNotBeNullOrEmpty();
            result.RefreshToken.ShouldNotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenuserNotExists()
    {
        var context = new TestContext();
    
        var request = new LoginRequestJsonBuilder()
            .Build();
        context.Add(request);
        
        var useCase = CreateUseCase(password: request.Password);

        await _testLogger.ExecuteWithContext(context, async () =>
        {
           var result = await useCase.Execute(request)
            .ShouldThrowAsync<InvalidLoginException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.Unauthorized);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(ResourceErrorMessages.VALIDATION_INVALID_LOGIN);
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenIncorrectPassword()
    {
        var context = new TestContext();

        var (user, password) = new UserBuilder()
            .Build();
    
        var request = new LoginRequestJsonBuilder()
            .WithEmail(user.Email)
            .Build();
        context.Add(request);
        
        var useCase = CreateUseCase(user);

        await _testLogger.ExecuteWithContext(context, async () =>
        {
           var result = await useCase.Execute(request)
            .ShouldThrowAsync<InvalidLoginException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.Unauthorized);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(ResourceErrorMessages.VALIDATION_INVALID_LOGIN);
        });
    }

    private static LoginWithEmailAndPasswordUseCase CreateUseCase(CinemaBackend.Domain.Entities.UserEntities.User? user = null, string? password = null)
    {
        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
        if (user != null)
            userReadOnlyRepositoryBuilder.GetUserByEmail(user);
        
        var passwordHasherBuilder = new IPasswordHasherBuilder();
        if (password != null)
            passwordHasherBuilder.VerifyPassword(password);
        
        var accessTokenGenerator = new IUserAccessTokenGeneratorBuilder()
            .Build();

        var refreshTokenGenerator = new IUserRefreshTokenGeneratorBuilder()
            .Build();

        return new LoginWithEmailAndPasswordUseCase
        (
            passwordHasher: passwordHasherBuilder.Build(),
            userReadOnlyRepository: userReadOnlyRepositoryBuilder.Build(),
            accessTokenGenerator: accessTokenGenerator,
            refreshTokenGenerator: refreshTokenGenerator
        );
    }
}
