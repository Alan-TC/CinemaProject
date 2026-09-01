using FluentValidation;

namespace CinemaBackend.Admin.Application.Shared;

internal static class EmailValidator
{
    internal static IRuleBuilderOptions<T, string> Email<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");
    }
}