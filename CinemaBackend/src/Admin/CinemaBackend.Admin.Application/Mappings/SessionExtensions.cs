using CinemaBackend.Communication.Enums;
using CinemaBackend.Communication.Responses.Session;
using CinemaBackend.Domain.Entities.SessionEntities;
using CinemaBackend.Domain.Storage.Movie;

namespace CinemaBackend.Admin.Application.Mappings;

internal static class SessionExtensions
{
    internal static SessionResponseJson MapToSessionResponseJson(this Session entity, IMovieImageStorage movieImageStorage)
    {
        return new SessionResponseJson
        {
            Id = entity.Id,
            Code = entity.Code,
            StartsAt = entity.StartsAt,
            EndsAt = entity.EndsAt,
            StartTicketSalesAt= entity.StartTicketSalesAt,
            Pricing = entity.Pricing.NormalPrice,
            Movie = entity.Movie.MapToMovieResponseJson(movieImageStorage),
            SessionRoom = new SessionRoomResponseJson {
                Tag = entity.SessionRoom.Tag,
                Width = entity.SessionRoom.Width,
                Height = entity.SessionRoom.Height,
                Seats = entity.SessionRoom.Seats
                    .Select(s => new SeatReponseJson
                    {
                        Code = s.Code,
                        PosX = s.PosX,
                        PosY = s.PosY,
                        Status = (SeatStatus)s.Status
                    }).ToList()
            }
        };
    }
}
