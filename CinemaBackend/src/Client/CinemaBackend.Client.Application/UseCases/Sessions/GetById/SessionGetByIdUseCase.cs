using CinemaBackend.Client.Application.Mappings;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Responses.Session;
using CinemaBackend.Domain.Repositories.Session;
using CinemaBackend.Domain.Storage.Movie;

namespace CinemaBackend.Client.Application.UseCases.Sessions.GetById;

public class SessionGetByIdUseCase(
    ISessionReadOnlyRepository sessionReadOnlyRepository,
    IMovieImageStorage movieImageStorage
)
    : ISessionGetByIdUseCase
{
    private readonly ISessionReadOnlyRepository _sessionReadOnlyRepository = sessionReadOnlyRepository;
    private readonly IMovieImageStorage _movieImageStorage = movieImageStorage;

    public async Task<SessionResponseJson> Execute(Guid sessionId)
    {
        var session = await _sessionReadOnlyRepository.GetById(sessionId, true);

        if (session is null)
            throw new NotFoundException();

        return session.MapToSessionResponseJson(_movieImageStorage);
    }
}
