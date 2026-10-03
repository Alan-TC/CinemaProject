using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.SharedKernel.Extensions;
using Shouldly;
using System.Net;
using System.Text.Json;
using TestsCommonUtilities;
using TestsCommonUtilities.Identity;
using TestsCommonUtilities.Requests;
using Xunit.Abstractions;

namespace Admin.WebApi.Tests.Authentication.RefreshToken;

public class RefreshTokenTests : IClassFixture<AdminWebApplicationFactory>
{
    private const string REQUEST_URI = "/authentication/refresh";

    private readonly TestWebRequest<Program> _testRequest;

    private readonly TestLoggingClass _testLogger;

    private readonly AdminUserIdentityManager _adminUser;

    public RefreshTokenTests(AdminWebApplicationFactory factory, ITestOutputHelper output)
    {
        _testRequest = new TestWebRequest<Program>(factory);

        _testLogger = new TestLoggingClass(output);

        _adminUser = factory.AdminUser;
    }

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        context.Add(_adminUser);

        var request = new RefreshTokenRequestJsonBuilder()
            .WithRefreshToken(_adminUser.GetRefreshToken())
            .Build();
        context.Add(request);

        await _testLogger.ExecuteWithContext(context, async () =>
        {
           var response = await _testRequest.Post(REQUEST_URI, request);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            var accessToken = responseData.RootElement.GetProperty("accessToken").GetString();
            accessToken.ShouldNotBeNullOrEmpty();

            var refreshToken = responseData.RootElement.GetProperty("refreshToken").GetString();
            refreshToken.ShouldNotBeNullOrEmpty();

            accessToken.ShouldNotBe(refreshToken);
            refreshToken.ShouldBe(request.RefreshToken);
        });
    }

    [Fact]
    public async Task Validate_ShouldBeUnauthorizedErrorResponse_WhenInvalidRefreshToken()
    {
        var context = new TestContext();

        context.Add(_adminUser);

        var request = new RefreshTokenRequestJsonBuilder()
            .Build();
        context.Add(request);

        await _testLogger.ExecuteWithContext(context, async() =>
        {
            var response = await _testRequest.Post(REQUEST_URI, request);

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
            errors.Count().ShouldBe(1);
            errors.ShouldContain(error => 
                error.GetString().IsNotEmpty() && 
                error.GetString()!.Equals(ResourceErrorMessages.VALIDATION_REFRESH_TOKEN_INVALID));
        });
    }

    [Fact]
    public async Task Validate_ShouldBeUnauthorizedErrorResponse_WhenAccessTokenIsPassed()
    {
        var context = new TestContext();

        context.Add(_adminUser);

        var request = new RefreshTokenRequestJsonBuilder()
            .WithRefreshToken(_adminUser.GetAccessToken())
            .Build();
        context.Add(request);

        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var response = await _testRequest.Post(REQUEST_URI, request);

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
            errors.Count().ShouldBe(1);
            errors.ShouldContain(error =>
                error.GetString().IsNotEmpty() &&
                error.GetString()!.Equals(ResourceErrorMessages.VALIDATION_REFRESH_TOKEN_INVALID));
        });
    }
}
