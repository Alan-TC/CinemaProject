using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.Domain.Repositories.User;
using CinemaBackend.Domain.Security.PasswordHasher;

namespace CinemaBackend.Client.Application.UseCases.User.Register;

public class RegisterUserUseCase(
    IUserReadOnlyRepository userReadOnlyRepository,
    IUserWriteOnlyRepository userWriteOnlyRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork) : IRegisterUserUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository = userWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    private readonly IPasswordHasher _passwordHasher = passwordHasher;

    public async Task Execute(UserRequestJson request)
    {
        await ValidateAndThrowOnFailures(request);

        var user = new Domain.Entities.UserEntities.User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = _passwordHasher.HashPassword(request.Password)
        };

        await _userWriteOnlyRepository.AddUser(user);
        await _unitOfWork.Commit();
    }

    private async Task ValidateAndThrowOnFailures(UserRequestJson request)
    {
        var validator = new UserRequestJsonValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            throw new ErrorOnValidationException([.. result.Errors.Select(error => error.ErrorMessage) ]);
        }

        var userExists = await _userReadOnlyRepository.UserEmailExists(request.Email);

        if (userExists == true)
        {
            throw new ConflicException(ResourceErrorMessages.VALIDATION_EMAIL_EXISTS);
        }
    }
}
