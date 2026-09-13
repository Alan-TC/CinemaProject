using CinemaBackend.Communication.Responses.Session;

namespace CinemaBackend.Admin.Application.UseCases.Session;

public interface ISessionGetByIdUseCase
{
    Task<SessionResponseJson> Execute(Guid sessionId);
}
