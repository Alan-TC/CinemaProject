using CinemaBackend.Communication.Requests;
using CinemaBackend.Domain.Entities.TheaterLayoutEntities;
using CinemaBackend.Domain.Enums;

namespace CinemaBackend.Admin.Application.Mappings;

internal static class TheaterLayoutRequestJsonExtensions
{
    internal static TheaterLayout MapToTheaterLayout(this TheaterLayoutRequestJson request)
    {
        return new TheaterLayout
        {
            Tag = request.Tag,
            Layout = new LayoutGrid
            {
                Width = request.Layout.Width,
                Height = request.Layout.Height,
                Grid = [.. request.Layout
                    .Grid.Select(g => new GridCell {PosX = g.PosX, PosY = g.PosY, Type = (CellType)g.Type}) ]
            }
        };
    }
}
