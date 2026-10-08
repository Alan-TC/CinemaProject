using CinemaBackend.Communication.Responses.CinemaTickets;

namespace CinemaBackend.Client.Application.UseCases.CinemaTickets.GetById;

public interface IGetUserCinemaTicketByIdUseCase
{
    Task<CinemaTicketResponseJson> Execute(Guid id);
}
