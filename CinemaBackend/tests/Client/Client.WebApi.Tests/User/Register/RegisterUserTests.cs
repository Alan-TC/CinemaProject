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

namespace Client.WebApi.Tests.User.Register;

public class RegisterUserTests : IClassFixture<ClientWebApplicationFactory>
{
    private const string REQUEST_URI = "/users";

    private readonly TestWebRequest<Program> _testRequest;

    private readonly TestLoggingClass _testLogger;

    private readonly UserIdentityManager _user;

    public RegisterUserTests(ClientWebApplicationFactory factory, ITestOutputHelper output)
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

        var request = new UserRequestJsonBuilder()
            .WithEmail("diff" + _user.GetEmail())
            .Build();
        context.Add(request);

        await _testLogger.ExecuteWithContext(context, async () =>
        {
           var response = await _testRequest.Post(REQUEST_URI, request);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            responseBody.Length.ShouldBe(0);

            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            var existsUser = await _testRequest.DbContext
                .Users
                .AnyAsync(u => u.Name == request.Name && u.Email == request.Email && u.DeactivatedAt == null);

            existsUser.ShouldBeTrue();
        });
    }

    [Fact]
    public async Task Validate_ShouldBeUnauthorizedErrorResponse_WhenEmailExists()
    {
        var context = new TestContext();

        context.Add(_user);

        var request = new UserRequestJsonBuilder()
            .WithEmail(_user.GetEmail())
            .Build();
        context.Add(request);

        await _testLogger.ExecuteWithContext(context, async() =>
        {
            var response = await _testRequest.Post(REQUEST_URI, request);

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            response.StatusCode.ShouldBe(HttpStatusCode.Conflict);
            errors.Count().ShouldBe(1);
            errors.ShouldContain(error => 
                error.GetString().IsNotEmpty() && 
                error.GetString()!.Equals(ResourceErrorMessages.VALIDATION_EMAIL_EXISTS));
        });
    }
}
