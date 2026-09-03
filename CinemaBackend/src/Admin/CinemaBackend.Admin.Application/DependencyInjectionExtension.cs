using CinemaBackend.Admin.Application.UseCases.AdminUser.Register;
using CinemaBackend.Admin.Application.UseCases.Authentication.Login.WithEmailAndPassword;
using CinemaBackend.Admin.Application.UseCases.Authentication.RefreshToken;
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
        services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();

        services.AddScoped<IRegisterUserAdminUseCase, RegisterUserAdminUseCase>();
    }
}
