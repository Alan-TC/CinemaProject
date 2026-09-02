using System.Net;
using CinemaBackend.Admin.Application.UseCases.Authentication.Login.WithEmailAndPassword;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.ResourceFiles.Errors;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Entities;
using TestsCommonUtilities.Repositories.AdminUser;
using TestsCommonUtilities.Requests;
using TestsCommonUtilities.Security;
using Xunit.Abstractions;

namespace UseCases.Tests.Authentication.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCaseTests(ITestOutputHelper output) : TestBaseClass(output)
{
    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        var (adminUser, password) = new AdminUserBuilder()
            .Build();
        context.Add(adminUser);

        var request = new LoginRequestJsonBuilder()
            .WithEmail(adminUser.Email)
            .WithPassword(password)
            .Build();
        context.Add(context);

        var useCase = CreateUseCase(adminUser, request.Password);

        await ExecuteWithContext(context, async () =>
        {
            var result = await useCase.Execute(request);

            result.AccessToken.ShouldNotBeNullOrEmpty();
            result.RefreshToken.ShouldNotBeNullOrEmpty();
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenAdminUserNotExists()
    {
        var context = new TestContext();
    
        var request = new LoginRequestJsonBuilder()
            .Build();
        context.Add(request);
        
        var useCase = CreateUseCase(password: request.Password);

        await ExecuteWithContext(context, async () =>
        {
           var result = await useCase.Execute(request)
            .ShouldThrowAsync<InvalidLoginException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.Unauthorized);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(ResourceErrorMessages.VALIDATE_INVALID_LOGIN);
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenIncorrectPassword()
    {
        var context = new TestContext();

        var (adminUser, password) = new AdminUserBuilder()
            .Build();
    
        var request = new LoginRequestJsonBuilder()
            .WithEmail(adminUser.Email)
            .Build();
        context.Add(request);
        
        var useCase = CreateUseCase(adminUser);

        await ExecuteWithContext(context, async () =>
        {
           var result = await useCase.Execute(request)
            .ShouldThrowAsync<InvalidLoginException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.Unauthorized);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(ResourceErrorMessages.VALIDATE_INVALID_LOGIN);
        });
    }

    private static LoginWithEmailAndPasswordUseCase CreateUseCase(CinemaBackend.Domain.Entities.AdminUser? adminUser = null, string? password = null)
    {
        var adminUserReadOnlyRepositoryBuilder = new IAdminUserReadOnlyRepositoryBuilder();
        if (adminUser != null)
            adminUserReadOnlyRepositoryBuilder.GetAdminUserByEmail(adminUser);
        
        var passwordHasherBuilder = new IPasswordHasherBuilder();
        if (password != null)
            passwordHasherBuilder.VerifyPassword(password);
        
        var accessTokenGenerator = new IAccessTokenGeneratorBuilder()
            .Build();

        var refreshTokenGenerator = new IRefreshTokenGeneratorBuilder()
            .Build();

        return new LoginWithEmailAndPasswordUseCase
        (
            passwordHasher: passwordHasherBuilder.Build(),
            adminUserReadOnlyRepository: adminUserReadOnlyRepositoryBuilder.Build(),
            accessTokenGenerator: accessTokenGenerator,
            refreshTokenGenerator: refreshTokenGenerator
        );
    }
}
