using System.Net;
using CinemaBackend.Admin.Application.UseCases.Movie.UpdateDetails;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.Domain.Enums;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Entities;
using TestsCommonUtilities.Repositories;
using TestsCommonUtilities.Repositories.Movie;
using TestsCommonUtilities.Requests;
using Xunit.Abstractions;

namespace Admin.UseCases.Tests.Movie.UpdateDetails;

public class UpdateMovieDetailsUseCaseTests(ITestOutputHelper output)
{
    private readonly TestLoggingClass _testLogger = new(output);

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        var movie = new MovieBuilder()
            .Build();
        context.Add(movie);

        var request = new MovieRequestJsonBuilder()
            .Build();
        context.Add(request);
        
        var useCase = CreateUseCase(movie);

        await _testLogger.ExecuteWithContext(context, async () =>
        {
           await useCase.Execute(movie.Id, request).ShouldNotThrowAsync();

           movie.Title.ShouldBe(request.Title);
           movie.Sinopse.ShouldBe(request.Sinopse);
           movie.Duration.ShouldBe(request.Duration);
           movie.ReleaseDate.ShouldBe(request.ReleaseDate);
           movie.ClassInd.ShouldBe((ClassInd)request.ClassInd);
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowAnException_WhenMovieDoesntExists()
    {
        var context = new TestContext();

        var request = new MovieRequestJsonBuilder()
            .Build();
        context.Add(request);
        
        var useCase = CreateUseCase();

        await _testLogger.ExecuteWithContext(context, async () =>
        {
           var result = await useCase.Execute(Guid.CreateVersion7(), request).ShouldThrowAsync<NotFoundException>();

           result.GetErrorCode().ShouldBe(HttpStatusCode.NotFound);
           result.GetErrorMessages().Count.ShouldBe(1);
           result.GetErrorMessages()
            .ShouldContain(error => error.Equals(ResourceErrorMessages.RESOURCE_NOT_FOUND));
        });
    }

    private static UpdateMovieDetailsUseCase CreateUseCase(CinemaBackend.Domain.Entities.Movie? movie = null)
    {
        var movieUpdateOnlyRepositoryBuilder = new IMovieUpdateOnlyRepositoryBuilder();
        var unitOfWork = new IUnitOfWorkBuilder().Build();

        if (movie != null)
            movieUpdateOnlyRepositoryBuilder.GetMovieById(movie);

        return new UpdateMovieDetailsUseCase(
            movieUpdateOnlyRepository: movieUpdateOnlyRepositoryBuilder.Build(),
            unitOfWork: unitOfWork);
    }
}
