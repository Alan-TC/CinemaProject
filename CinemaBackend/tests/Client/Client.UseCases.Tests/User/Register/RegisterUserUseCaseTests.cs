using System.Net;
using CinemaBackend.Client.Application.UseCases.Users.Register;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.ResourceFiles.Errors;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Repositories;
using TestsCommonUtilities.Repositories.User;
using TestsCommonUtilities.Requests;
using TestsCommonUtilities.Security;
using Xunit.Abstractions;

namespace Client.UseCases.Tests.User.Register;

public class RegisterUserUseCaseTests(ITestOutputHelper output)
{
    private readonly TestLoggingClass _testLogger = new(output);

    [Fact]
    public async Task Success()
    {
        var request = new UserRequestJsonBuilder()
            .Build();

        var useCase = CreateUseCase();

        await _testLogger.ExecuteWithContext(request, async () =>
        {
           await useCase.Execute(request).ShouldNotThrowAsync(); 
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenEmailAlreadyExists()
    {
        var context = new TestContext();

        var request = new UserRequestJsonBuilder()
            .Build();
        context.Add(request);

        var useCase = CreateUseCase(request.Email);

        await _testLogger.ExecuteWithContext(context, async() =>
        {
           var result = await useCase.Execute(request).ShouldThrowAsync<ConflictException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.Conflict);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(message => message.Equals(ResourceErrorMessages.VALIDATION_EMAIL_EXISTS));
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenNameIsEmpty()
    {
        var context = new TestContext();

        var request = new UserRequestJsonBuilder()
            .WithName(string.Empty)
            .Build();
        context.Add(request);

        var useCase = CreateUseCase();

        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var result = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.BadRequest);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(message => message.Equals(ResourceErrorMessages.VALIDATION_NAME_REQUIRED));
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenPasswordIsEmpty()
    {
        var context = new TestContext();

        var request = new UserRequestJsonBuilder()
            .WithPassword(string.Empty)
            .Build();
        context.Add(request);

        var useCase = CreateUseCase();

        await _testLogger.ExecuteWithContext(context, async() =>
        {
            var result = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.BadRequest);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(message => message.Equals(ResourceErrorMessages.VALIDATION_PASSWORD_REQUIRED));
        });
    }

    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var userWriteOnlyRepository = new IUserWriteOnlyRepositoryBuilder().Build();
        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
        var passwordHasher = new IPasswordHasherBuilder().Build();
        var unitOfWork = new IUnitOfWorkBuilder().Build();
        
        if (email is not null)
        {
            userReadOnlyRepositoryBuilder.UserEmailExists(email);
        }

        return new RegisterUserUseCase
        (
            userWriteOnlyRepository: userWriteOnlyRepository,
            userReadOnlyRepository: userReadOnlyRepositoryBuilder.Build(),
            passwordHasher: passwordHasher,
            unitOfWork: unitOfWork
        );
    }
}
