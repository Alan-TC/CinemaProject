using System.Diagnostics.CodeAnalysis;
using CinemaBackend.Admin.Application.UseCases.Movie.UpdateDetails;
using CinemaBackend.Communication.Enums;
using CinemaBackend.Communication.ResourceFiles.Errors;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Requests;
using Xunit.Abstractions;

namespace Admin.Validators.Tests.Movie.UpdateDetails;

public class MovieRequestJsonValidatorTests(ITestOutputHelper output)
{
    private readonly TestLoggingClass _testLogger = new(output);

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        var request = new MovieRequestJsonBuilder()
            .Build();

        context.Add(request);

        var validator = new MovieRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
            result.Errors.ShouldBeEmpty();
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData("         ")]
    [InlineData(null)]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters")]
    public async Task Validate_ShouldHaveAnError_WhenTitleIsEmpty(string title)
    {
        var context = new TestContext();

        var request = new MovieRequestJsonBuilder()
            .WithTitle(title)
            .Build();

        context.Add(request);

        var validator = new MovieRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_MOVIE_TITLE_REQUIRED));
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData("         ")]
    [InlineData(null)]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters")]
    public async Task Validate_ShouldHaveAnError_WhenSinopseIsEmpty(string sinopse)
    {
        var context = new TestContext();

        var request = new MovieRequestJsonBuilder()
            .WithSinopse(sinopse)
            .Build();

        context.Add(request);

        var validator = new MovieRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_MOVIE_SINOPSE_REQUIRED));
        });
    }

    [Fact]
    public async Task Validate_ShouldHaveAnError_WhenClasIndIsInvalid()
    {
        var context = new TestContext();

        var request = new MovieRequestJsonBuilder()
            .WithClassInd((ClassInd)1000)
            .Build();

        context.Add(request);

        var validator = new MovieRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_MOVIE_CLASS_IND_INVALID));
        });
    }

    [Fact]
    public async Task Validate_ShouldHaveAnError_WhenGenreIsInvalid()
    {
        var context = new TestContext();

        var request = new MovieRequestJsonBuilder()
            .WithFilmGenres([(Genres)1000])
            .Build();

        context.Add(request);

        var validator = new MovieRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_MOVIE_GENRES_INVALID));
        });
    }
}
