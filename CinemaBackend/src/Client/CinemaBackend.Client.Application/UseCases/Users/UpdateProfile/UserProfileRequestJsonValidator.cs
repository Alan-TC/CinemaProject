using CinemaBackend.Client.Application.Shared;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.ResourceFiles.Errors;
using FluentValidation;

namespace CinemaBackend.Client.Application.UseCases.Users.UpdateProfile;

public class UserProfileRequestJsonValidator : AbstractValidator<UserProfileRequestJson>
{
    public UserProfileRequestJsonValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_NAME_REQUIRED);

        RuleFor(request => request.Email)
            .Email();
    }
}
