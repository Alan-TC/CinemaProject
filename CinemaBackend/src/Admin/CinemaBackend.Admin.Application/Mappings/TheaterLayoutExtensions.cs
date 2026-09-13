using System.Text;
using CinemaBackend.Communication.Enums;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Communication.SharedRequestResponse;
using CinemaBackend.Domain.Entities.SessionEntities;
using CinemaBackend.Domain.Entities.TheaterLayoutEntities;

namespace CinemaBackend.Admin.Application.Mappings;

internal static class TheaterLayoutExtensions
{
    internal static TheaterLayoutResponseJson MapToTheaterLayoutResponseJson(this TheaterLayout entity)
    {
        return new TheaterLayoutResponseJson
        {
           Id =  entity.Id,
           Tag = entity.Tag,
           CreatedAt = entity.CreatedAt,
           Layout = new LayoutGridJson
           {
               Height = entity.Layout.Height,
               Width = entity.Layout.Width,
               Grid = [.. entity.Layout.Grid.Select(g => new Communication.SharedRequestResponse.GridCell 
                {
                  PosX = g.PosX,
                  PosY = g.PosY,
                  Type = (CellType)g.Type
                })]
           }
        };
    }

    internal static SessionRoom MapToSessionRoom(this TheaterLayout entity)
    {
        return new SessionRoom
        {
           Tag = entity.Tag,
           Width = entity.Layout.Width,
           Height = entity.Layout.Height,
           Seats = entity.Layout.Grid
            .Where(g => g.Type == Domain.Enums.CellType.Seat)
            .Select(g => new SessionRoomSeat
            {
                Code = IntToLettersZeroIndexed(g.PosY) + (g.PosX + 1).ToString(),
                PosX = g.PosX,
                PosY = g.PosY,
                Status = Domain.Enums.SeatStatus.Available
            }).ToList()
        };
    }

    private static string IntToLettersZeroIndexed(int value)
    {
        var retorno = new StringBuilder();

        value++;
        while (value > 0)
        {
            value--;
            retorno.Insert(0, (char)('A' + value%26));
            value /= 26;
        }

        return retorno.ToString();
    }
}
