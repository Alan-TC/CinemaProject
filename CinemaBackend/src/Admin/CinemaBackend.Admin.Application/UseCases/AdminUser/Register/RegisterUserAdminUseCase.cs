using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.Domain.Repositories.AdminUser;
using CinemaBackend.Domain.Security.PasswordHasher;

namespace CinemaBackend.Admin.Application.UseCases.AdminUser.Register;

public class RegisterUserAdminUseCase(
    IAdminUserWriteOnlyRepository adminUserWriteOnlyRepository,
    IAdminUserReadOnlyRepository adminUserReadOnlyRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher) : IRegisterUserAdminUseCase
{
    private readonly IAdminUserWriteOnlyRepository _adminUserWriteOnlyRepository = adminUserWriteOnlyRepository;
    private readonly IAdminUserReadOnlyRepository _adminUserReadOnlyRepository = adminUserReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    
    public async Task Execute(RegisterUserAdminRequestJson request)
    {
        await ValidateAndThrowOnFailures(request);

        var passwordHash = _passwordHasher.HashPassword(request.Password);

        var adminUser = new Domain.Entities.AdminUserEntities.AdminUser
        {
            Email = request.Email,
            PasswordHash = passwordHash
        };

        await _adminUserWriteOnlyRepository.AddAdminUser(adminUser);

        await _unitOfWork.Commit();
    }

    private async Task ValidateAndThrowOnFailures(RegisterUserAdminRequestJson request)
    {
        var validator = new RegisterUserAdminValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());

        var userExists = await _adminUserReadOnlyRepository.AdminUserEmailExists(request.Email);

        if (userExists)
            throw new ConflictException(ResourceErrorMessages.VALIDATION_EMAIL_EXISTS);
    }
}
