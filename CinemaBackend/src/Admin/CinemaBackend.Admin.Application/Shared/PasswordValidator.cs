using FluentValidation;

namespace CinemaBackend.Admin.Application.Shared;

internal static class PasswordValidator
{
    internal static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long.");
    }
}