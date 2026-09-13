using CinemaBackend.Admin.Application.Mappings;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.Communication.Responses.Session;
using CinemaBackend.Domain.Entities.SessionEntities;
using CinemaBackend.Domain.Repositories.Movie;
using CinemaBackend.Domain.Repositories.Session;
using CinemaBackend.Domain.Repositories.TheaterLayout;
using CinemaBackend.Domain.Storage.Movie;
using CinemaBackend.SharedKernel.Services;

namespace CinemaBackend.Admin.Application.UseCases.Session.Create;

public class SessionCreateUseCase(
    ISessionReadOnlyRepository sessionReadOnlyRepository,
    ISessionWriteOnlyRepository sessionWriteOnlyRepository,
    ITheaterLayoutReadOnlyRepository theaterLayoutReadOnlyRepository,
    IMovieUpdateOnlyRepository movieUpdateOnlyRepository,
    IMovieImageStorage movieImageStorage,
    IUnitOfWork unitOfWork
) : ISessionCreateUseCase
{
    private readonly ISessionReadOnlyRepository _sessionReadOnlyRepository = sessionReadOnlyRepository;
    private readonly ISessionWriteOnlyRepository _sessionWriteOnlyRepository = sessionWriteOnlyRepository;
    private readonly ITheaterLayoutReadOnlyRepository _theaterLayoutReadOnlyRepository = theaterLayoutReadOnlyRepository;
    private readonly IMovieUpdateOnlyRepository _movieUpdateOnlyRepository = movieUpdateOnlyRepository;
    private readonly IMovieImageStorage _movieImageStorage = movieImageStorage;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<SessionResponseJson> Execute(SessionRequestJson request)
    {
        ValidateAndThrowOnFailures(request);

        var theaterLayout = await _theaterLayoutReadOnlyRepository.GetById(request.TheaterLayoutId);

        if (theaterLayout is null)
            throw new ErrorOnValidationException([ ResourceErrorMessages.VALIDATION_SESSION_INVALID_THEATER_LAYOUT ]);
        
        var movie = await _movieUpdateOnlyRepository.GetMovieById(request.MovieId);

        if (movie is null)
            throw new ErrorOnValidationException([ ResourceErrorMessages.VALIDATION_SESSION_INVALID_MOVIE ]);

        var code = await GenerateValidCode();

        var sessionRoom = theaterLayout.MapToSessionRoom();

        var session = new Domain.Entities.SessionEntities.Session
        {
            Code = code,
            StartsAt = request.StartsAt,
            EndsAt = request.EndsAt,
            StartTicketSalesAt = request.StartTicketSalesAt,
            Pricing = new SessionPricing
            {
                NormalPrice = request.Pricing
            },
            Movie = movie,
            SessionRoom = sessionRoom
        };
        
        await _sessionWriteOnlyRepository.Add(session);

        await _unitOfWork.Commit();

        return session.MapToSessionResponseJson(_movieImageStorage);
    }

    public async Task<string> GenerateValidCode()
    {
        var code = "";

        do
        {
            code = CodeGenerator.GenerateCode(8);
        } while(await _sessionReadOnlyRepository.HasSessionWithCode(code));

        return code;
    }

    private static void ValidateAndThrowOnFailures(SessionRequestJson request)
    {
        var validator = new SessionRequestJsonValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
            throw new ErrorOnValidationException([.. result.Errors.Select(error => error.ErrorMessage)]);
    }
}
