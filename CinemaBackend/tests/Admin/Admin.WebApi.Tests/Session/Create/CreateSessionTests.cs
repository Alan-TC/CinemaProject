using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using System.Net;
using System.Text.Json;
using TestsCommonUtilities;
using TestsCommonUtilities.Identity;
using TestsCommonUtilities.Requests;
using Xunit.Abstractions;

namespace Admin.WebApi.Tests.Session.Create;

public class CreateSessionTests(ITestOutputHelper output, AdminWebApplicationFactory factory) : IClassFixture<AdminWebApplicationFactory>
{
    private const string REQUEST_URI = "/sessions";
    private readonly TestLoggingClass _testLogger = new(output);

    private readonly TestWebRequest<Program> _testRequest = new(factory);
    private readonly AdminUserIdentityManager _admin = factory.AdminUser;
    private readonly CinemaBackend.Domain.Entities.TheaterLayoutEntities.TheaterLayout _theaterLayout = factory.TheaterLayout;
    private readonly CinemaBackend.Domain.Entities.MovieEntities.Movie _movie = factory.Movie;

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        context.Add(_theaterLayout);
        context.Add(_movie);

        var request = new SessionRequestJsonBuilder()
            .WithTheaterId(_theaterLayout.Id)
            .WithMovieId(_movie.Id)
            .Build();
        context.Add(request);

        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var response = await _testRequest.Post(REQUEST_URI, request, _admin.GetAccessToken());

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var sessionId = responseData.RootElement.GetProperty("id").ToString();

            var id = Guid.Parse(sessionId);

            var dbResult = await _testRequest.DbContext
                .Sessions.AsNoTracking().AsSplitQuery()
                .Include(s => s.Movie)
                .Include(s => s.SessionRoom)
                    .ThenInclude(sr => sr.Seats)
                .FirstOrDefaultAsync(s => s.Id == id && s.DeactivatedAt == null);

            dbResult.ShouldNotBeNull();
            dbResult.Movie.Id.ShouldBe(request.MovieId);
            dbResult.SessionRoom.Tag.ShouldBe(_theaterLayout.Tag);
        });
    }

    [Fact]
    public async Task Validate_ShouldBeBadRequest_WhenTheaterLayoutNotExists()
    {
        var context = new TestContext();

        context.Add(_movie);

        var request = new SessionRequestJsonBuilder()
            .WithMovieId(_movie.Id)
            .Build();
        context.Add(request);

        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var response = await _testRequest.Post(REQUEST_URI, request, _admin.GetAccessToken());

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            errors.Count().ShouldBe(1);
            errors.ShouldContain(error =>
                error.GetString().IsNotEmpty() &&
                error.GetString()!.Equals(ResourceErrorMessages.VALIDATION_SESSION_INVALID_THEATER_LAYOUT));
        });
    }
}
