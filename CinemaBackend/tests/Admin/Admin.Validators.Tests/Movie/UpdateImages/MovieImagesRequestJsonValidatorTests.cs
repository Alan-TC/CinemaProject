using CinemaBackend.Admin.Application.UseCases.Movie.UpdateImages;
using CinemaBackend.Communication.ResourceFiles.Errors;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Requests;
using Xunit.Abstractions;

namespace Admin.Validators.Tests.Movie.UpdateImages;

public class MovieImagesRequestJsonValidatorTests(ITestOutputHelper output)
{
    private readonly TestLoggingClass _testLogger = new(output);

    [Fact]
    public async Task Success_WhenImage()
    {
        var context = new TestContext();

        var request = new MovieImagesRequestJsonBuilder()
            .Build();

        context.Add(request);

        var validator = new MovieImagesRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
            result.Errors.ShouldBeEmpty();
        });
    }

    [Theory]
    [InlineData(ImageProperty.Poster)]
    [InlineData(ImageProperty.Featured)]
    [InlineData(ImageProperty.Standard)]
    public async Task Success_WhenImageIsPng(ImageProperty property)
    {
        var context = new TestContext();

        var fileType = FileType.PNG;
        var request = new MovieImagesRequestJsonBuilder()
            .ConfigureImage(property, fileType)
            .Build();

        context.Add(request);

        var validator = new MovieImagesRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
            result.Errors.ShouldBeEmpty();
        });
    }

    [Theory]
    [InlineData(ImageProperty.Poster)]
    [InlineData(ImageProperty.Featured)]
    [InlineData(ImageProperty.Standard)]
    public async Task Success_WhenImageIsJpeg(ImageProperty property)
    {
        var context = new TestContext();

        var fileType = FileType.JPEG;
        var request = new MovieImagesRequestJsonBuilder()
            .ConfigureImage(property, fileType)
            .Build();

        context.Add(request);

        var validator = new MovieImagesRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
            result.Errors.ShouldBeEmpty();
        });
    }

    [Theory]
    [InlineData(ImageProperty.Poster)]
    [InlineData(ImageProperty.Featured)]
    [InlineData(ImageProperty.Standard)]
    public async Task Validate_ShouldHaveAnError_WhenImageIsInvalid(ImageProperty property)
    {
        var context = new TestContext();

        var fileType = FileType.BMP;
        var request = new MovieImagesRequestJsonBuilder()
            .ConfigureImage(property, fileType)
            .Build();

        context.Add(request);

        var validator = new MovieImagesRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_INVALID_IMAGE));
        });
    }

    [Theory]
    [InlineData(ImageProperty.Poster)]
    [InlineData(ImageProperty.Featured)]
    [InlineData(ImageProperty.Standard)]
    public async Task Validate_ShouldHaveAnError_WhenImageIsTxt(ImageProperty property)
    {
        var context = new TestContext();

        var fileType = FileType.TXT;
        var request = new MovieImagesRequestJsonBuilder()
            .ConfigureImage(property, fileType)
            .Build();

        context.Add(request);

        var validator = new MovieImagesRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_INVALID_IMAGE));
        });
    }
}

public enum ImageProperty
{
    Poster,
    Featured,
    Standard
}

public static class MovieImagesRequestJsonBuilderConfigureImage
{
    public static MovieImagesRequestJsonBuilder ConfigureImage(this MovieImagesRequestJsonBuilder r, ImageProperty property, string fileType)
    {
        if (property == ImageProperty.Poster)
            r.WithPoster(fileType);
        else if (property == ImageProperty.Featured)
            r.WithFeaturedBanner(fileType);
        else if (property == ImageProperty.Standard)
            r.WithStandardBanner(fileType);

        return r;
    }
}