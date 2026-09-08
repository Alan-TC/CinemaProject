using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;

namespace CinemaBackend.Admin.Application.UseCases.TheaterLayout.Create;

public interface ICreateTheaterLayoutUseCase
{
    Task<TheaterLayoutResponseJson> Execute(TheaterLayoutRequestJson request);
}
