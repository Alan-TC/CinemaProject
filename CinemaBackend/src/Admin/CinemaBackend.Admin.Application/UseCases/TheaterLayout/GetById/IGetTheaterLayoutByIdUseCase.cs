using CinemaBackend.Communication.Responses;

namespace CinemaBackend.Admin.Application.UseCases.TheaterLayout.GetById;

public interface IGetTheaterLayoutByIdUseCase
{
    Task<TheaterLayoutResponseJson> Execute(Guid theaterId);
}
