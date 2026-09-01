using CinemaBackend.Admin.Application.Shared;
using CinemaBackend.Communication.Requests;
using FluentValidation;

namespace CinemaBackend.Admin.Application.UseCases.Authentication.Register;

public class RegisterUserAdminValidator : AbstractValidator<RegisterUserAdminRequest>
{
    public RegisterUserAdminValidator()
    {
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.Password).Password();
    }
}