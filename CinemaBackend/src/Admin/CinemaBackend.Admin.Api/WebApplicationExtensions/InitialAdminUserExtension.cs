using CinemaBackend.Admin.Application.UseCases.AdminUser.Register;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Requests;
using CinemaBackend.SharedKernel.Extensions;

namespace CinemaBackend.Admin.Api.WebApplicationExtensions;

public static class InitialAdminUser
{
    public static async Task RegisterInitialAdminUser(this WebApplication app)
    {
        var configuration = app.Configuration;

        var userAdminEmail = configuration["InitialAdmin:Email"];
        var userAdminPassword = configuration["InitialAdmin:Password"];

        if (userAdminEmail.IsEmpty() && userAdminPassword.IsEmpty())
            return;

        await using var scope = app.Services.CreateAsyncScope();

        var registerUseAdminUseCase = scope.ServiceProvider.GetRequiredService<IRegisterUserAdminUseCase>();

        var request = new RegisterUserAdminRequestJson 
            {
                Email = userAdminEmail!, 
                Password = userAdminPassword!
            };
        
        try
        {
            await registerUseAdminUseCase.Execute(request);
        }
        catch (ConflicException)
        {
            return;
        }
        catch (ErrorOnValidationException ex)
        {
            var logger = app.Services.GetRequiredService<ILogger<Program>>();

            foreach (var message in ex.GetErrorMessages())
            {
                logger.LogCritical("Error on validation: {ErrorMessage}", message);
            }

            Environment.Exit(1);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
