using Bogus;
using Bogus.Extensions;
using CinemaBackend.Communication.Enums;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.SharedRequestResponse;

namespace TestsCommonUtilities.Requests;

public class TheaterLayoutRequestJsonBuilder
{
    private readonly Faker<TheaterLayoutRequestJson> _faker = new();

    private int wStart = 1;
    private int wEnd = 10;
    private int hStart = 1;
    private int hEnd = 10;

    private List<GridCell>? GridCells = null;

    public TheaterLayoutRequestJsonBuilder()
    {
        _faker.RuleFor(r => r.Tag, f => f.Lorem.Word().ClampLength(3, 8));
        _faker.RuleFor(r => r.Layout, LayoutWithRange());
    }

    public TheaterLayoutRequestJsonBuilder WithTag(string tag)
    {
        _faker.RuleFor(r => r.Tag, tag);
        return this;
    }

    public TheaterLayoutRequestJsonBuilder WithWidth(int start, int end)
    {
        wStart = start;
        wEnd = end;

        return this;
    }

    public TheaterLayoutRequestJsonBuilder WithHeight(int start, int end)
    {
        hStart = start;
        hEnd = end;

        return this;
    }

    public TheaterLayoutRequestJsonBuilder WithGridCell(List<GridCell> gridCells)
    {
        GridCells = gridCells;

        return this;
    }

    private LayoutGridJson LayoutWithRange()
    {
        var random = new Random();
        var width = random.Next(wStart, wEnd);
        var height = random.Next(hStart, hEnd);

        var grid = new List<GridCell>();

        if (GridCells is null)
        {
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    grid.Add(new GridCell{ PosX = x, PosY = y, Type = CellType.Seat});
                }
            }
                
        }
        else {
            grid = GridCells;
        }

        return new LayoutGridJson
        {
            Width = width,
            Height = height,
            Grid = grid
        };
    }

    public TheaterLayoutRequestJson Build() 
    {
        _faker.RuleFor(r => r.Layout, LayoutWithRange());
        return _faker.Generate();
    }
}
