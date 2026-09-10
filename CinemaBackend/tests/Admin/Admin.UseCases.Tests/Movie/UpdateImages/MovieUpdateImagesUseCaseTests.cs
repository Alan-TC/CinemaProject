using System.Net;
using CinemaBackend.Admin.Application.UseCases.Movie.UpdateImages;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.ResourceFiles.Errors;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Entities;
using TestsCommonUtilities.Repositories.Movie;
using TestsCommonUtilities.Requests;
using TestsCommonUtilities.Storage.Movie;
using Xunit.Abstractions;

namespace Admin.UseCases.Tests.Movie.UpdateImages;

public class MovieUpdateImagesUseCaseTests(ITestOutputHelper output)
{
    private readonly TestLoggingClass _testLogger = new(output);

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        var movie = new MovieBuilder()
            .Build();
        context.Add(movie);

        var request = new MovieImagesRequestJsonBuilder()
            .WithMovieId(movie.Id)
            .Build();
        context.Add(request);

        var useCase = CreateUseCase(movie);

        await _testLogger.ExecuteWithContext(context, async () =>
        {
            await useCase.Execute(request).ShouldNotThrowAsync();
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowAnException_WhenMovieDoesntExists()
    {
        var context = new TestContext();

        var request = new MovieImagesRequestJsonBuilder()
            .Build();
        context.Add(request);

        var useCase = CreateUseCase();

        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var result = await useCase.Execute(request).ShouldThrowAsync<NotFoundException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.NotFound);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(error => error.Equals(ResourceErrorMessages.RESOURCE_NOT_FOUND));
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowAnException_WhenInvalidImage()
    {
        var context = new TestContext();

        var request = new MovieImagesRequestJsonBuilder()
            .WithPoster(FileType.BMP)
            .Build();
        context.Add(request);

        var useCase = CreateUseCase();

        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var result = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.BadRequest);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(error => error.Equals(ResourceErrorMessages.VALIDATION_INVALID_IMAGE));
        });
    }

    private static MovieUpdateImagesUseCase CreateUseCase(CinemaBackend.Domain.Entities.MovieEntities.Movie? movie = null)
    {
        var movieReadOnlyRepositoryBuilder = new IMovieReadOnlyRepositoryBuilder();
        var movieImageStorage = new IMovieImageStorageBuilder()
            .Build();

        if (movie != null)
            movieReadOnlyRepositoryBuilder.GetMovieById(movie);


        return new MovieUpdateImagesUseCase
        (
            movieReadOnlyRepository: movieReadOnlyRepositoryBuilder.Build(),
            movieImageStorage: movieImageStorage
        );
    }
}
