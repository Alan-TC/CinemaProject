using CinemaBackend.Client.Application.UseCases.Authentication.Login.WithEmailAndPassword;
using CinemaBackend.Client.Application.UseCases.Authentication.RefreshToken;
using CinemaBackend.Client.Application.UseCases.CinemaTickets.GetById;
using CinemaBackend.Client.Application.UseCases.CinemaTickets.GetUserCinemaTickets;
using CinemaBackend.Client.Application.UseCases.Movies.GetById;
using CinemaBackend.Client.Application.UseCases.Movies.GetMovies;
using CinemaBackend.Client.Application.UseCases.Movies.GetMovieSessions;
using CinemaBackend.Client.Application.UseCases.Payments.GetPayment;
using CinemaBackend.Client.Application.UseCases.Payments.OrderPayment;
using CinemaBackend.Client.Application.UseCases.Payments.PayOrder;
using CinemaBackend.Client.Application.UseCases.Sessions.GetById;
using CinemaBackend.Client.Application.UseCases.Sessions.GetSessions;
using CinemaBackend.Client.Application.UseCases.Users.GetProfile;
using CinemaBackend.Client.Application.UseCases.Users.Register;
using CinemaBackend.Client.Application.UseCases.Users.UpdateProfile;
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
        services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
        services.AddScoped<IUpdateUserProfileUseCase, UpdateUserProfileUseCase>();

        services.AddScoped<IRefreshTokenUseCase, RefreshTokenUseCase>();
        services.AddScoped<ILoginWithEmailAndPasswordUseCase, LoginWithEmailAndPasswordUseCase>();

        services.AddScoped<ISessionGetByIdUseCase, SessionGetByIdUseCase>();
        services.AddScoped<IGetSessionsFilteredPagedUseCase, GetSessionsFilteredPagedUseCase>();

        services.AddScoped<IMoviesGetFilteredPagedUseCase,  MoviesGetFilteredPagedUseCase>();
        services.AddScoped<IMovieGetByIdUseCase,  MovieGetByIdUseCase>();
        services.AddScoped<IGetMovieSessionsFilteredPagedUseCase, GetMovieSessionsFilteredPagedUseCase>();

        services.AddScoped<IOrderPaymentUseCase, OrderPaymentUseCase>();
        services.AddScoped<IGetPaymentUseCase, GetPaymentUseCase>();
        services.AddScoped<IPayOrderUseCase, PayOrderUseCase>();

        services.AddScoped<IGetUserCinemaTicketsUseCase, GetUserCinemaTicketsUseCase>();
        services.AddScoped<IGetUserCinemaTicketByIdUseCase, GetUserCinemaTicketByIdUseCase>();
    }
}