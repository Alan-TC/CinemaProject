using System.Diagnostics.CodeAnalysis;
using CinemaBackend.Admin.Application.UseCases.TheaterLayout;
using CinemaBackend.Communication.Enums;
using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.Communication.SharedRequestResponse;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Requests;
using Xunit.Abstractions;

namespace Admin.Validators.Tests.TheaterLayout;

public class TheaterLayoutRequestJsonValidatorTests(ITestOutputHelper output)
{
    private readonly TestLoggingClass _testLogger = new(output);

    [Fact]
    public void Success()
    {
        var context = new TestContext();

        var request = new TheaterLayoutRequestJsonBuilder()
            .Build();
        context.Add(request);

        var validator = new TheaterLayoutRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
            result.Errors.ShouldBeEmpty();
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData("        ")]
    [InlineData(null)]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters")]
    public void Validate_ShouldHaveAnError_WhenTagIsEmpty(string tag)
    {
        var context = new TestContext();

        var request = new TheaterLayoutRequestJsonBuilder()
            .WithTag(tag)
            .Build();
        context.Add(request);

        var validator = new TheaterLayoutRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => 
                error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_TAG_REQUIRED));
        });
    }

    [Fact]
    public void Validate_ShouldHaveAnError_WhenHeightIsLower()
    {
        var context = new TestContext();

        var request = new TheaterLayoutRequestJsonBuilder()
            .WithHeight(-10, 0)
            .Build();
        context.Add(request);

        var validator = new TheaterLayoutRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(error => 
                error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_HEIGHT_MIN_VALUE));
        });
    }

    [Fact]
    public void Validate_ShouldHaveAnError_WhenWidthIsLower()
    {
        var context = new TestContext();

        var request = new TheaterLayoutRequestJsonBuilder()
            .WithWidth(-10, 0)
            .Build();
        context.Add(request);

        var validator = new TheaterLayoutRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(error => 
                error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_WIDTH_MIN_VALUE));
        });
    }

    [Theory]
    [InlineData(-10)]
    [InlineData(-1)]
    [InlineData(10)]
    [InlineData(11)]
    public void Validate_ShouldHaveAnError_WhenPosXInvalid(int pos)
    {
        var context = new TestContext();

        var gridCells = new List<GridCell> { new() {
           PosX =  pos,
           PosY = 8,
           Type = CellType.Seat
        }};

        var request = new TheaterLayoutRequestJsonBuilder()
            .WithWidth(1, 10)
            .WithHeight(1, 10)
            .WithGridCell(gridCells)
            .Build();
        context.Add(request);

        var validator = new TheaterLayoutRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(error => 
                error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_GRID_POSX_RANGE_VALUE));
        });
    }

    [Theory]
    [InlineData(-10)]
    [InlineData(-1)]
    [InlineData(10)]
    [InlineData(11)]
    public void Validate_ShouldHaveAnError_WhenPosYInvalid(int pos)
    {
        var context = new TestContext();

        var gridCells = new List<GridCell> { new() {
           PosX =  8,
           PosY = pos,
           Type = CellType.Seat
        }};

        var request = new TheaterLayoutRequestJsonBuilder()
            .WithWidth(1, 10)
            .WithHeight(1, 10)
            .WithGridCell(gridCells)
            .Build();
        context.Add(request);

        var validator = new TheaterLayoutRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(error => 
                error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_GRID_POSY_RANGE_VALUE));
        });
    }

    [Fact]
    public void Validate_ShouldHaveAnError_WhenGridCellNotUnique()
    {
        var context = new TestContext();

        var gridCells = new List<GridCell> { 
            new() {
                PosX =  8,
                PosY = 8,
                Type = CellType.Seat
            },
            new() {
                PosX =  8,
                PosY = 8,
                Type = CellType.Seat
            }
        };

        var request = new TheaterLayoutRequestJsonBuilder()
            .WithWidth(1, 10)
            .WithHeight(1, 10)
            .WithGridCell(gridCells)
            .Build();
        context.Add(request);

        var validator = new TheaterLayoutRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(error => 
                error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_GRID_POSY_RANGE_VALUE));
        });
    }
}
