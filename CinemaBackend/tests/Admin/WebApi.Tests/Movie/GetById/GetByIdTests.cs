using System.Net;
using System.Text.Json;
using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.SharedKernel.Extensions;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Identity;
using Xunit.Abstractions;

namespace Admin.WebApi.Tests.Movie.GetById;

public class GetByIdTests(ITestOutputHelper output, AdminWebApplicationFactory factory) : IClassFixture<AdminWebApplicationFactory>
{
    private const string REQUEST_URI = "/movies";
    private readonly TestLoggingClass _testLogger = new(output);

    private readonly TestWebRequest<Program> _testRequest = new(factory);
    private readonly AdminUserIdentityManager _admin = factory.AdminUser;
    private readonly CinemaBackend.Domain.Entities.Movie _movie = factory.Movie;

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        context.Add(_movie);

        await _testLogger.ExecuteWithContext(context, async () =>
        {
           var response = await _testRequest.Get(REQUEST_URI + $"/{_movie.Id}", _admin.GetAccessToken());

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var movieId = responseData.RootElement.GetProperty("id").ToString();
            var movieTitle = responseData.RootElement.GetProperty("title").ToString();
            var movieSinopse = responseData.RootElement.GetProperty("sinopse").ToString();

            movieId.ShouldBe(_movie.Id.ToString());
            movieTitle.ShouldBe(_movie.Title);
            movieSinopse.ShouldBe(_movie.Sinopse);
        });
    }

    [Fact]
    public async Task Validate_ShouldBeNotFound_WhenMovieDoesntExists()
    {
        var context = new TestContext();

        context.Add(_movie);

        await _testLogger.ExecuteWithContext(context, async () =>
        {
           var response = await _testRequest.Get(REQUEST_URI + $"/{Guid.CreateVersion7()}", _admin.GetAccessToken());

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
}
