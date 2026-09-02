using System.Net;
using CinemaBackend.Admin.Application.UseCases.AdminUser.Register;
using CinemaBackend.Communication.Exceptions;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Repositories;
using TestsCommonUtilities.Repositories.AdminUser;
using TestsCommonUtilities.Requests;
using TestsCommonUtilities.Security;
using Xunit.Abstractions;

namespace UseCases.Tests.AdminUser.Register;

public class RegisterUserAdminUseCaseTests(ITestOutputHelper testOutput) : TestBaseClass(testOutput)
{
    [Fact]
    public async Task Success()
    {
        var request = new RegisterUserAdminRequestJsonBuilder()
            .Build();

        var useCase = CreateUseCase();

        await ExecuteWithContext(request, async () =>
        {
           await useCase.Execute(request).ShouldNotThrowAsync(); 
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenEmailAlreadyExists()
    {
        var context = new TestContext();

        var request = new RegisterUserAdminRequestJsonBuilder()
            .Build();
        context.Add(request);

        var useCase = CreateUseCase(request.Email);

        await ExecuteWithContext(context, async() =>
        {
           var result = await useCase.Execute(request).ShouldThrowAsync<ConflicException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.Conflict);
            result.GetErrorMessages().Count.ShouldBe(1); 
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenPasswordIsEmpty()
    {
        var context = new TestContext();

        var request = new RegisterUserAdminRequestJsonBuilder()
            .WithPassword(string.Empty)
            .Build();
        context.Add(request);

        var useCase = CreateUseCase();

        await ExecuteWithContext(context, async() =>
        {
            var result = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.BadRequest);
            result.GetErrorMessages().Count.ShouldBe(1);
        });
    }

    private static RegisterUserAdminUseCase CreateUseCase(string? email = null)
    {
        var adminUserWriteOnlyRepository = new IAdminUserWriteOnlyRepositoryBuilder().Build();
        var adminUserReadOnlyRepositoryBuilder = new IAdminUserReadOnlyRepositoryBuilder();
        var passwordHasher = new IPasswordHasherBuilder().Build();
        var unitOfWork = new IUnitOfWorkBuilder().Build();
        
        if (email is not null)
        {
            adminUserReadOnlyRepositoryBuilder.AdminUserEmailExists(email);
        }

        return new RegisterUserAdminUseCase
        (
            adminUserWriteOnlyRepository: adminUserWriteOnlyRepository,
            adminUserReadOnlyRepository: adminUserReadOnlyRepositoryBuilder.Build(),
            passwordHasher: passwordHasher,
            unitOfWork: unitOfWork
        );
    }
}
