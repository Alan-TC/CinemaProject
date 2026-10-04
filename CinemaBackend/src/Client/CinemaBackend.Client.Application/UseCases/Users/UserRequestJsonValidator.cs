using CinemaBackend.Client.Application.Shared;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.ResourceFiles.Errors;
using FluentValidation;

namespace CinemaBackend.Client.Application.UseCases.Users;

public class UserRequestJsonValidator : AbstractValidator<UserRequestJson>
{
    public UserRequestJsonValidator()
    {
        RuleFor(request => request.Name)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_NAME_REQUIRED);

        RuleFor(request => request.Email)
            .Email();

        RuleFor(request => request.Password)
            .Password();
    }
}
