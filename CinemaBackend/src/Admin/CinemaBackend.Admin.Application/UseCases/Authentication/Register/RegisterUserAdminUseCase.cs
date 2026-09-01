using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Requests;

namespace CinemaBackend.Admin.Application.UseCases.Authentication.Register;

public class RegisterUserAdminUseCase : IRegisterUserAdminUseCase
{
    public void Execute(RegisterUserAdminRequest request)
    {
        ValidateAndThrowOnFailures(request);

        
    }

    private static void ValidateAndThrowOnFailures(RegisterUserAdminRequest request)
    {
        var validator = new RegisterUserAdminValidator();
        var result = validator.Validate(request);

        if (!result.IsValid)
            throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
    }
}
