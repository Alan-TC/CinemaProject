using CinemaBackend.Communication.Responses.Session;

namespace CinemaBackend.Client.Application.UseCases.Sessions.GetById;

public interface ISessionGetByIdUseCase
{
    Task<SessionResponseJson> Execute(Guid sessionId);
}
