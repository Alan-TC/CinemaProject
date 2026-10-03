using System.Net;
using System.Text.Json;
using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.SharedKernel.Extensions;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Requests;
using TestsCommonUtilities.Identity;
using Xunit.Abstractions;

namespace Client.WebApi.Tests.Authentication.Login.WithEmailAndPassword;

public class WithEmailAndPasswordTests : IClassFixture<ClientWebApplicationFactory>
{
    private const string REQUEST_URI = "/authentication/login";

    private readonly TestWebRequest<Program> _testRequest;

    private readonly TestLoggingClass _testLogger;

    private readonly UserIdentityManager _user;

    public WithEmailAndPasswordTests(ClientWebApplicationFactory factory, ITestOutputHelper output)
    {
        _testRequest = new TestWebRequest<Program>(factory);

        _testLogger = new TestLoggingClass(output);

        _user = factory.User;
    }

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        context.Add(_user);

        var request = new LoginRequestJsonBuilder()
            .WithEmail(_user.GetEmail())
            .WithPassword(_user.GetPassword())
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
        });
    }

    [Fact]
    public async Task Validate_ShouldBeUnauthorizedErrorResponse_WhenUserNotExist()
    {
        var context = new TestContext();

        context.Add(_user);

        var request = new LoginRequestJsonBuilder()
            .WithEmail("m" + _user.GetEmail())
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
                error.GetString()!.Equals(ResourceErrorMessages.VALIDATION_INVALID_LOGIN));
        });
    }
}
