using CinemaBackend.Communication.Enums;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Communication.SharedRequestResponse;
using CinemaBackend.Domain.Entities.TheaterLayoutEntities;

namespace CinemaBackend.Admin.Application.Mappings;

public static class TheaterLayoutExtensions
{
    public static TheaterLayoutResponseJson MapToTheaterLayoutResponseJson(this TheaterLayout entity)
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
}
