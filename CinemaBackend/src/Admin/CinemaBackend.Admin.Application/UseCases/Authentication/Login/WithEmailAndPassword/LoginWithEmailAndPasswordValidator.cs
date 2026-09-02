using CinemaBackend.Admin.Application.Shared;
using CinemaBackend.Communication.Requests;
using FluentValidation;

namespace CinemaBackend.Admin.Application.UseCases.Authentication.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordValidator : AbstractValidator<LoginRequestJson>
{
    public LoginWithEmailAndPasswordValidator()
    {
        RuleFor(r => r.Email).Email();
        RuleFor(r => r.Password).Password();
    }
}
