using Bogus;
using Bogus.Extensions;
using CinemaBackend.Domain.Entities.TheaterLayoutEntities;
using CinemaBackend.Domain.Enums;

namespace TestsCommonUtilities.Entities;

public class TheaterLayoutBuilder
{
    private readonly Faker<TheaterLayout> _faker = new();

    private int wStart = 1;
    private int wEnd = 10;
    private int hStart = 1;
    private int hEnd = 10;

    private GridCell? gridCell = null;

    public TheaterLayoutBuilder()
    {
        _faker.RuleFor(r => r.Tag, f => f.Lorem.Word().ClampLength(3, 8));
        _faker.RuleFor(r => r.Layout, LayoutWithRange());
    }

    public TheaterLayoutBuilder WithTag(string tag)
    {
        _faker.RuleFor(r => r.Tag, tag);
        return this;
    }

    public TheaterLayoutBuilder WithWidth(int start, int end)
    {
        wStart = start;
        wEnd = end;

        return this;
    }

    public TheaterLayoutBuilder WithHeight(int start, int end)
    {
        hStart = start;
        hEnd = end;

        return this;
    }

    public TheaterLayoutBuilder WithGridCell(int x, int y, CellType type = CellType.Seat)
    {
        gridCell = new GridCell
        {
            PosX = x,
            PosY = y,
            Type = type
        };

        return this;
    }

    private LayoutGrid LayoutWithRange()
    {
        var random = new Random();
        var width = random.Next(wStart, wEnd);
        var height = random.Next(hStart, hEnd);

        var grid = new List<GridCell>();

        if (gridCell is null)
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
            grid.Add(gridCell);
        }

        return new LayoutGrid
        {
            Width = width,
            Height = height,
            Grid = grid
        };
    }

    public TheaterLayout Build() 
    {
        _faker.RuleFor(r => r.Layout, LayoutWithRange());
        return _faker.Generate();
    }
}
