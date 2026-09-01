using System.Net;
using CinemaBackend.Admin.Application.UseCases.AdminUser.Register;
using CinemaBackend.Communication.Exceptions;
using Shouldly;
using TestsCommonUtilities.Repositories;
using TestsCommonUtilities.Repositories.AdminUser;
using TestsCommonUtilities.Requests;
using TestsCommonUtilities.Security;

namespace UseCases.Tests.AdminUser.Register;

public class RegisterUserAdminUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var request = new RegisterUserAdminRequestJsonBuilder()
            .Build();

        var useCase = CreateUseCase();

        await useCase.Execute(request).ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenEmailAlreadyExists()
    {
        var request = new RegisterUserAdminRequestJsonBuilder()
            .Build();

        var useCase = CreateUseCase(request.Email);

        var result = await useCase.Execute(request).ShouldThrowAsync<ConflicException>();

        result.GetErrorCode().ShouldBe(HttpStatusCode.Conflict);
        result.GetErrorMessages().Count.ShouldBe(1);
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenPasswordIsEmpty()
    {
        var request = new RegisterUserAdminRequestJsonBuilder()
            .WithPassword(string.Empty)
            .Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        result.GetErrorCode().ShouldBe(HttpStatusCode.BadRequest);
        result.GetErrorMessages().Count.ShouldBe(1);
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
