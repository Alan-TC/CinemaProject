using CinemaBackend.Admin.Application.UseCases.Authentication.Login.WithEmailAndPassword;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaBackend.Admin.Application;

public static class DependencyInjectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddUseCases();
    }

    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ILoginWithEmailAndPasswordUseCase, LoginWithEmailAndPasswordUseCase>();
    }
}
