using System.Net;
using CinemaBackend.Admin.Application.UseCases.Movie.Create;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.ResourceFiles.Errors;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Repositories;
using TestsCommonUtilities.Repositories.Movie;
using TestsCommonUtilities.Requests;
using TestsCommonUtilities.Storage.Movie;
using Xunit.Abstractions;

namespace Admin.UseCases.Tests.Movie.Create;

public class CreateMovieUseCaseTests(ITestOutputHelper output)
{
    private readonly TestLoggingClass _testLogger = new(output);

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        var request = new CreateMovieUseCaseRequestBuilder()
            .Build();
        context.Add(request);
        
        var useCase = CreateUseCase();

        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var result = await useCase.Execute(request);

            result.Title.ShouldBe(request.MovieDetails.Title);
            result.Sinopse.ShouldBe(request.MovieDetails.Sinopse);
        });
    }

    [Fact]
    public async Task Success_WithPosterIsPng()
    {
        var context = new TestContext();

        var request = new CreateMovieUseCaseRequestBuilder()
            .WithPoster(FileType.PNG)
            .Build();
        context.Add(request);
        
        var useCase = CreateUseCase();

        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var result = await useCase.Execute(request);

            result.Title.ShouldBe(request.MovieDetails.Title);
            result.Sinopse.ShouldBe(request.MovieDetails.Sinopse);
        });
    }

    [Fact]
    public async Task Success_ShouldThrowException_WhenTitleIsEmpty()
    {
        var context = new TestContext();

        var request = new CreateMovieUseCaseRequestBuilder()
            .WithTitle(string.Empty)
            .Build();
        context.Add(request);
        
        var useCase = CreateUseCase();

        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var result = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.BadRequest);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(error => error.Equals(ResourceErrorMessages.VALIDATION_MOVIE_TITLE_REQUIRED));
        });
    }

    private static CreateMovieUseCase CreateUseCase()
    {
        var movieWriteOnlyRepository = new IMovieWriteOnlyRepositoryBuilder()
            .Build();

        var movieImageStorageBuilder = new IMovieImageStorageBuilder()
            .GetMovieImagesUrl()
            .Build();

        var unitOfWork = new IUnitOfWorkBuilder()
            .Build();
        
        return new CreateMovieUseCase(movieWriteOnlyRepository, movieImageStorageBuilder, unitOfWork);
    }
}
