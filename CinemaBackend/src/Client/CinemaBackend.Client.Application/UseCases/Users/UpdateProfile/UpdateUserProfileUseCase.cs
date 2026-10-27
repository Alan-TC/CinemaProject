using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.Domain.Identity.Client;
using CinemaBackend.Domain.Repositories.User;

namespace CinemaBackend.Client.Application.UseCases.Users.UpdateProfile;

public class UpdateUserProfileUseCase(
    IUserReadOnlyRepository userReadOnlyRepository,
    IUserUpdateOnlyRepository userUpdateOnlyRepository,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork) : IUpdateUserProfileUseCase
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository = userUpdateOnlyRepository;

    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(UserProfileRequestJson request)
    {
        var user = await _userUpdateOnlyRepository.GetUserById(_loggedUser.GetUserId());

        if (user == null)
        {
            throw new NotFoundException();
        }

        if (user.Email == request.Email && user.Name == request.Name)
            return;

        await ValidateAndThrowOnFailures(request, user.Email);

        user.Email = request.Email;
        user.Name = request.Name;

        await _unitOfWork.Commit();
    }

    private async Task ValidateAndThrowOnFailures(UserProfileRequestJson request, string currEmail)
    {
        var validator = new UserProfileRequestJsonValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            throw new ErrorOnValidationException([.. result.Errors.Select(error => error.ErrorMessage) ]);
        }

        if (currEmail == request.Email)
            return;

        var emailExists = await _userReadOnlyRepository.UserEmailExists(request.Email);

        if (emailExists == true)
        {
            throw new ConflictException(ResourceErrorMessages.VALIDATION_EMAIL_EXISTS);
        }
    }
}
