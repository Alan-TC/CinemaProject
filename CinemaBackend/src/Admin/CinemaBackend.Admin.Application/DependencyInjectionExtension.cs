using CinemaBackend.Admin.Application.UseCases.AdminUser.Register;
using CinemaBackend.Admin.Application.UseCases.Authentication.Login.WithEmailAndPassword;
using CinemaBackend.Admin.Application.UseCases.Authentication.RefreshToken;
using CinemaBackend.Admin.Application.UseCases.Movie.Create;
using CinemaBackend.Admin.Application.UseCases.Movie.GetById;
using CinemaBackend.Admin.Application.UseCases.Movie.GetMovies;
using CinemaBackend.Admin.Application.UseCases.Movie.UpdateDetails;
using CinemaBackend.Admin.Application.UseCases.Movie.UpdateImages;
using CinemaBackend.Admin.Application.UseCases.TheaterLayout.Create;
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

        services.AddScoped<ICreateMovieUseCase, CreateMovieUseCase>();
        services.AddScoped<IMovieUpdateImagesUseCase, MovieUpdateImagesUseCase>();
        services.AddScoped<IUpdateMovieDetailsUseCase, UpdateMovieDetailsUseCase>();
        services.AddScoped<IMoviesGetFilteredPagedUseCase, MoviesGetFilteredPagedUseCase>();
        services.AddScoped<IMovieGetByIdUseCase, MovieGetByIdUseCase>();

        services.AddScoped<ICreateTheaterLayoutUseCase, CreateTheaterLayoutUseCase>();
    }
}
