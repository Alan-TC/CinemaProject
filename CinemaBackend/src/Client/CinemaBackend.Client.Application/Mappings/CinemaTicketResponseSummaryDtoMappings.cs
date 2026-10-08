using CinemaBackend.Communication.Responses.CinemaTickets;
using CinemaBackend.Domain.DTOs.CinemaTicket;

namespace CinemaBackend.Client.Application.Mappings;

public static class CinemaTicketSummaryDtoMappings
{
    public static CinemaTicketResponseSummaryJson MapToCinemaTicketResponseSummaryJson(this CinemaTicketSummaryDto dto)
    {
        return new CinemaTicketResponseSummaryJson
        {
            Id = dto.Id,
            Code = dto.Code,
            TheaterRoomTag = dto.TheaterRoomTag,
            StartsAt = dto.StartsAt,
            SeatsCodes = dto.SeatsCodes,
            HalfPriceTickets = dto.HalfPriceTickets
        };
    }
}
