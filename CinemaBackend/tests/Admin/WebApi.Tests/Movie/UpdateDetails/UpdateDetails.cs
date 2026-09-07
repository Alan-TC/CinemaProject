using System.Net;
using System.Text.Json;
using Admin.WebApi.Tests;
using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.Domain.Enums;
using CinemaBackend.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Identity;
using TestsCommonUtilities.Requests;
using Xunit.Abstractions;

namespace WebApi.Tests.Movie.UpdateDetails;

public class UpdateDetails(ITestOutputHelper output, AdminWebApplicationFactory factory) : IClassFixture<AdminWebApplicationFactory>
{
    private const string REQUEST_URI = "/movies";
    private readonly TestLoggingClass _testLogger = new(output);

    private readonly TestWebRequest<Program> _testRequest = new(factory);
    private readonly AdminUserIdentityManager _admin = factory.AdminUser;
    private readonly CinemaBackend.Domain.Entities.Movie _movieSuccessUpdate = factory.Movie;
    private readonly CinemaBackend.Domain.Entities.Movie _movieErrorUpdate = factory.Movie;

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        var request = new MovieRequestJsonBuilder()
            .Build();
        context.Add(request);
        
        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var response = await _testRequest.Put(Url(_movieSuccessUpdate.Id), request, _admin.GetAccessToken());

            var responseBody = await response.Content.ReadAsStringAsync();

            responseBody.ShouldBeEmpty();

            response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

            var movieResult = await _testRequest.DbContext.Movies
                .Include(m => m.FilmGenres)
                .FirstOrDefaultAsync(m => m.Id == _movieSuccessUpdate.Id && m.DeactivatedAt == null);
            
            movieResult.ShouldNotBeNull();
            movieResult.Title.ShouldBe(request.Title);
            movieResult.Sinopse.ShouldBe(request.Sinopse);
            movieResult.ClassInd.ShouldBe((ClassInd)request.ClassInd);
            movieResult.FilmGenres.Count.ShouldBe(request.FilmGenres.Count);
        });
    }

    [Fact]
    public async Task Validate_ShouldBeNotFound_WhenMovieDoesntExists()
    {
        var context = new TestContext();

        var request = new MovieRequestJsonBuilder()
            .Build();
        context.Add(request);
        
        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var response = await _testRequest.Put(Url(Guid.CreateVersion7()), request, _admin.GetAccessToken());

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

            errors.Count().ShouldBe(1);
            errors.ShouldContain(error => 
                error.GetString().IsNotEmpty() && 
                error.GetString()!.Equals(ResourceErrorMessages.RESOURCE_NOT_FOUND));
        });
    }

    [Fact]
    public async Task Validate_ShouldBeBadRequest_WhenTitleIsEmpty()
    {
        var context = new TestContext();

        var request = new MovieRequestJsonBuilder()
            .WithTitle(string.Empty)
            .Build();
        context.Add(request);
        
        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var response = await _testRequest.Put(Url(_movieErrorUpdate.Id), request, _admin.GetAccessToken());

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            errors.Count().ShouldBe(1);
            errors.ShouldContain(error => 
                error.GetString().IsNotEmpty() && 
                error.GetString()!.Equals(ResourceErrorMessages.VALIDATION_MOVIE_TITLE_REQUIRED));

            var movieResult = await _testRequest.DbContext.Movies
                .Include(m => m.FilmGenres)
                .FirstOrDefaultAsync(m => m.Id == _movieErrorUpdate.Id && m.DeactivatedAt == null);
            
            movieResult.ShouldNotBeNull();
            movieResult.Title.ShouldBe(_movieErrorUpdate.Title);
            movieResult.Sinopse.ShouldBe(_movieErrorUpdate.Sinopse);
            movieResult.ClassInd.ShouldBe(_movieErrorUpdate.ClassInd);
            movieResult.FilmGenres.Count.ShouldBe(_movieErrorUpdate.FilmGenres.Count);
        });
    }

    public static string Url(Guid movieId) => REQUEST_URI + "/" + movieId.ToString();
}
