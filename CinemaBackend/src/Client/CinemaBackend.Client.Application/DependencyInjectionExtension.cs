using CinemaBackend.Client.Application.UseCases.Authentication.Login.WithEmailAndPassword;
using CinemaBackend.Client.Application.UseCases.Authentication.RefreshToken;
using CinemaBackend.Client.Application.UseCases.User.Register;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaBackend.Client.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddUseCases(services);
    }
    
    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();

        services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
        services.AddScoped<ILoginWithEmailAndPasswordUseCase, LoginWithEmailAndPasswordUseCase>();
    }
}