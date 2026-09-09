using System.Net;
using System.Text.Json;
using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Identity;
using TestsCommonUtilities.Requests;
using Xunit.Abstractions;

namespace Admin.WebApi.Tests.TheaterLayout.Create;

public class CreateTheaterLayout(ITestOutputHelper output, AdminWebApplicationFactory factory) : IClassFixture<AdminWebApplicationFactory>
{
    private const string REQUEST_URI = "/theaterlayout";
    private readonly TestLoggingClass _testLogger = new(output);

    private readonly TestWebRequest<Program> _testRequest = new(factory);
    private readonly AdminUserIdentityManager _admin = factory.AdminUser;
    private readonly CinemaBackend.Domain.Entities.TheaterLayout _theaterLayout = factory.TheaterLayout;

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        var request = new TheaterLayoutRequestJsonBuilder()
            .Build();
        context.Add(request);
        
        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var response = await _testRequest.Post(REQUEST_URI, request, _admin.GetAccessToken());

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var thlaId = responseData.RootElement.GetProperty("id").ToString();

            var id = Guid.Parse(thlaId);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var dbResult = await _testRequest.DbContext
                .TheaterLayouts.FirstOrDefaultAsync(t => t.Id == id && t.DeactivatedAt == null);
            
            dbResult.ShouldNotBeNull();
            dbResult.Tag.ShouldBe(request.Tag);
        });        
    }

    [Fact]
    public async Task Validate_ShouldBeConflict_WhenTagExists()
    {
        var context = new TestContext();

        context.Add(_theaterLayout);
        var request = new TheaterLayoutRequestJsonBuilder()
            .WithTag(_theaterLayout.Tag)
            .Build();
        context.Add(request);
        
        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var response = await _testRequest.Post(REQUEST_URI, request, _admin.GetAccessToken());

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            response.StatusCode.ShouldBe(HttpStatusCode.Conflict);

            errors.Count().ShouldBe(1);
            errors.ShouldContain(error => 
                error.GetString().IsNotEmpty() && 
                error.GetString()!.Equals(ResourceErrorMessages.VALIDATION_THEATER_LAYOUT_TAG_ALREADY_EXISTS));
        });        
    }
}
