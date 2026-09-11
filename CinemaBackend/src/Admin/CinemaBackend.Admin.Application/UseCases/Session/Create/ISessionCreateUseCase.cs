using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses.Session;

namespace CinemaBackend.Admin.Application.UseCases.Session.Create;

public interface ISessionCreateUseCase
{
    Task<SessionResponseJson> Execute(SessionRequestJson request);
}
