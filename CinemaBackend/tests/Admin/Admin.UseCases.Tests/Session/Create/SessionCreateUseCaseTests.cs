using CinemaBackend.Admin.Application.UseCases.Session.Create;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.ResourceFiles.Errors;
using Shouldly;
using System.Net;
using TestsCommonUtilities;
using TestsCommonUtilities.Entities;
using TestsCommonUtilities.Repositories;
using TestsCommonUtilities.Repositories.Movie;
using TestsCommonUtilities.Repositories.Sessions;
using TestsCommonUtilities.Repositories.TheaterLayout;
using TestsCommonUtilities.Requests;
using TestsCommonUtilities.Storage.Movie;
using Xunit.Abstractions;

namespace Admin.UseCases.Tests.Session.Create;

public class SessionCreateUseCaseTests(ITestOutputHelper output)
{
    private readonly TestLoggingClass _testLogger = new(output);

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        var theaterLayout = new TheaterLayoutBuilder().Build();
        context.Add(theaterLayout);
        
        var movie = new MovieBuilder().Build();
        context.Add(movie);

        var request = new SessionRequestJsonBuilder()
            .WithTheaterId(theaterLayout.Id)
            .WithMovieId(movie.Id)
            .Build();
        context.Add(request);

        var useCase = CreateUseCase(movie, theaterLayout);

        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var result = await useCase.Execute(request);

            result.Movie.Id.ShouldBe(movie.Id);
            result.SessionRoom.Tag.ShouldBe(theaterLayout.Tag);
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenTheaterLayoutNotExists()
    {
        var context = new TestContext();

        var movie = new MovieBuilder().Build();
        context.Add(movie);

        var request = new SessionRequestJsonBuilder()
            .WithMovieId(movie.Id)
            .Build();
        context.Add(request);

        var useCase = CreateUseCase(movie);

        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var result = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.BadRequest);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(error => 
                error.Equals(ResourceErrorMessages.VALIDATION_SESSION_INVALID_THEATER_LAYOUT));
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenMovieNotExists()
    {
        var context = new TestContext();

        var theaterLayout = new TheaterLayoutBuilder().Build();
        context.Add(theaterLayout);

        var request = new SessionRequestJsonBuilder()
            .WithTheaterId(theaterLayout.Id)
            .Build();
        context.Add(request);

        var useCase = CreateUseCase(theater: theaterLayout);

        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var result = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.BadRequest);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(error =>
                error.Equals(ResourceErrorMessages.VALIDATION_SESSION_INVALID_MOVIE));
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenInvalidPrice()
    {
        var context = new TestContext();

        var theaterLayout = new TheaterLayoutBuilder().Build();
        context.Add(theaterLayout);

        var movie = new MovieBuilder().Build();
        context.Add(movie);

        var request = new SessionRequestJsonBuilder()
            .WithTheaterId(theaterLayout.Id)
            .WithMovieId(movie.Id)
            .WithPrice(-1)
            .Build();
        context.Add(request);

        var useCase = CreateUseCase(theater: theaterLayout);

        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var result = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.BadRequest);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(error =>
                error.Equals(ResourceErrorMessages.VALIDATION_SESSION_INVALID_PRICE));
        });
    }

    private static SessionCreateUseCase CreateUseCase(
        CinemaBackend.Domain.Entities.MovieEntities.Movie? movie = null,
        CinemaBackend.Domain.Entities.TheaterLayoutEntities.TheaterLayout? theater = null)
    {
        var sessionReadOnlyRepository = new ISessionReadOnlyRepositoryBuilder().Build();
        var sessionWriteOnlyRepository = new ISessionWriteOnlyRepositoryBuilder().Build();
        var theaterLayoutReadOnlyRepositoryBuilder = new ITheaterLayoutReadOnlyRepositoryBuilder();
        var movieUploadOnlyRepositoryBuilder = new IMovieUpdateOnlyRepositoryBuilder();
        var movieImageStorageBuilder = new IMovieImageStorageBuilder();
        var unitOfWork = new IUnitOfWorkBuilder().Build();

        if (movie != null)
        {
            movieUploadOnlyRepositoryBuilder.GetMovieById(movie);
            movieImageStorageBuilder.GetMovieImagesUrl(movie);
        }

        if (theater != null)
            theaterLayoutReadOnlyRepositoryBuilder.GetById(theater);

        return new SessionCreateUseCase
            (
                sessionReadOnlyRepository: sessionReadOnlyRepository,
                sessionWriteOnlyRepository: sessionWriteOnlyRepository,
                theaterLayoutReadOnlyRepository: theaterLayoutReadOnlyRepositoryBuilder.Build(),
                movieUpdateOnlyRepository: movieUploadOnlyRepositoryBuilder.Build(),
                movieImageStorage: movieImageStorageBuilder.Build(),
                unitOfWork: unitOfWork
            );
    }
}
