using System.Text;
using CinemaBackend.Domain.Entities.SessionEntities;
using CinemaBackend.Domain.Entities.TheaterLayoutEntities;

namespace TestsCommonUtilities.Entities;

public static class TheaterLayoutExtension
{
    internal static SessionRoom MapToSessionRoom(this TheaterLayout entity)
    {
        return new SessionRoom
        {
            Tag = entity.Tag,
            Width = entity.Layout.Width,
            Height = entity.Layout.Height,
            Seats = entity.Layout.Grid
                .Where(g => g.Type == CinemaBackend.Domain.Enums.CellType.Seat)
                .Select(g => new SessionRoomSeat
                {
                    Code = IntToLettersZeroIndexed(g.PosY) + (g.PosX + 1).ToString(),
                    PosX = g.PosX,
                    PosY = g.PosY,
                    Status = CinemaBackend.Domain.Enums.SeatStatus.Available
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