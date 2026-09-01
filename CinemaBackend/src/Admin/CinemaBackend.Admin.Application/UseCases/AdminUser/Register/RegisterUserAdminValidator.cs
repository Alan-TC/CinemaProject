using CinemaBackend.Admin.Application.Shared;
using CinemaBackend.Communication.Requests;
using FluentValidation;

namespace CinemaBackend.Admin.Application.UseCases.AdminUser.Register;

public class RegisterUserAdminValidator : AbstractValidator<RegisterUserAdminRequestJson>
{
    public RegisterUserAdminValidator()
    {
        RuleFor(x => x.Email).Email();
        RuleFor(x => x.Password).Password();
    }
}