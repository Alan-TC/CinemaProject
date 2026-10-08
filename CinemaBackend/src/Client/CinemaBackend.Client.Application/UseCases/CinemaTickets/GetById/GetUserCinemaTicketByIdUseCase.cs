using CinemaBackend.Client.Application.Mappings;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Responses.CinemaTickets;
using CinemaBackend.Domain.Identity.Client;
using CinemaBackend.Domain.Repositories.CinemaTicket;
using CinemaBackend.Domain.Storage.Movie;

namespace CinemaBackend.Client.Application.UseCases.CinemaTickets.GetById;

public class GetUserCinemaTicketByIdUseCase(
    ICinemaTicketReadOnlyRepository cinemaTicketReadOnlyRepository,
    IMovieImageStorage movieImageStorage,
    ILoggedUser loggedUser) : IGetUserCinemaTicketByIdUseCase
{
    private readonly ICinemaTicketReadOnlyRepository _cinemaTicketReadOnlyRepository = cinemaTicketReadOnlyRepository;
    private readonly IMovieImageStorage _movieImageStorage = movieImageStorage;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<CinemaTicketResponseJson> Execute(Guid id)
    {
        var cinemaTicket = await _cinemaTicketReadOnlyRepository.GetByIdFromUser(id, _loggedUser.GetUserId());

        if (cinemaTicket == null)
        {
            throw new NotFoundException();
        }

        return new CinemaTicketResponseJson
        {
            Id = id,
            Code = cinemaTicket.Code,
            TheaterRoomTag = cinemaTicket.TheaterRoomTag,
            HalfPriceTickets = cinemaTicket.HalfPriceTickets,
            SeatsCodes = cinemaTicket.SeatsCodes,
            StartsAt = cinemaTicket.StartsAt,
            Movie = cinemaTicket.Movie.MapToMovieResponseJson(_movieImageStorage)
        };
    }
}
