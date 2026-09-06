using FluentValidation;
using CinemaBackend.Communication.ResourceFiles.Errors;

namespace CinemaBackend.Admin.Application.Shared;

internal static class EmailValidator
{
    internal static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_EMAIL_REQUIRED)
            .EmailAddress()
            .WithMessage(ResourceErrorMessages.VALIDATION_EMAIL_INVALID);
    }
}