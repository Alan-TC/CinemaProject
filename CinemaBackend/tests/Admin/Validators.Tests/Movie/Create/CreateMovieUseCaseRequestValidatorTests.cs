using System.Diagnostics.CodeAnalysis;
using CinemaBackend.Admin.Application.UseCases.Movie.Create;
using CinemaBackend.Communication.ResourceFiles.Errors;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Requests;
using Xunit.Abstractions;

namespace Admin.Validators.Tests.Movie.Create;

public class CreateMovieUseCaseRequestValidatorTests(ITestOutputHelper output)
{
    private readonly TestLoggingClass _testLogger = new(output);
    
    [Fact]
    public async Task Success_WhenNoImage()
    {
        var context = new TestContext();

        var request = new CreateMovieUseCaseRequestBuilder()
            .Build();
        context.Add(request);
        
        var validator = new CreateMovieUseCaseRequestValidator();

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
        var request = new CreateMovieUseCaseRequestBuilder()
            .ConfigureImage(property, fileType)
            .Build();
        context.Add(request);

        var validator = new CreateMovieUseCaseRequestValidator();

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
        var request = new CreateMovieUseCaseRequestBuilder()
            .ConfigureImage(property, fileType)
            .Build();
        context.Add(request);

        var validator = new CreateMovieUseCaseRequestValidator();

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
    public async Task Validate_ShouldHaveAnError_WhenTitleIsEmpty(string title)
    {
        var context = new TestContext();

        var request = new CreateMovieUseCaseRequestBuilder()
            .WithTitle(title)
            .Build();
        context.Add(request);

        var validator = new CreateMovieUseCaseRequestValidator();
        
        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_MOVIE_TITLE_REQUIRED));
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData("        ")]
    [InlineData(null)]
    [SuppressMessage("Usage", "xUnit1012:Null should only be used for nullable parameters")]
    public async Task Validate_ShouldHaveAnError_WhenSinopseIsEmpty(string sinopse)
    {
        var context = new TestContext();

        var request = new CreateMovieUseCaseRequestBuilder()
            .WithSinopse(sinopse)
            .Build();
        context.Add(request);

        var validator = new CreateMovieUseCaseRequestValidator();
        
        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_MOVIE_SINOPSE_REQUIRED));
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
        var request = new CreateMovieUseCaseRequestBuilder()
            .ConfigureImage(property, fileType)
            .Build();
        context.Add(request);

        var validator = new CreateMovieUseCaseRequestValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
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
        var request = new CreateMovieUseCaseRequestBuilder()
            .ConfigureImage(property, fileType)
            .Build();
        context.Add(request);

        var validator = new CreateMovieUseCaseRequestValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
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

public static class CreateMovieUseCaseRequestBuilderConfigureImage
{
    public static CreateMovieUseCaseRequestBuilder ConfigureImage(this CreateMovieUseCaseRequestBuilder r, ImageProperty property, string fileType)
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