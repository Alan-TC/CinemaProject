using CinemaBackend.Communication.ResourceFiles.Errors;
using FluentValidation;

namespace CinemaBackend.Client.Application.Shared;

internal static class PasswordValidator
{
    internal static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_PASSWORD_REQUIRED)
            .MinimumLength(6)
            .WithMessage(ResourceErrorMessages.VALIDATION_PASSWORD_LENGHT);
    }
}