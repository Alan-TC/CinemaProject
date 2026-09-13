using System.Net;
using System.Text.Json;
using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Files;
using TestsCommonUtilities.Identity;
using TestsCommonUtilities.Requests;
using Xunit.Abstractions;

namespace Admin.WebApi.Tests.Movie.Create;

public class CreateMovie(ITestOutputHelper output, AdminWebApplicationFactory factory) : IClassFixture<AdminWebApplicationFactory>
{
    private const string REQUEST_URI = "/movies";
    private readonly TestLoggingClass _testLogger = new(output);

    private readonly TestWebRequest<Program> _testRequest = new(factory);
    private readonly AdminUserIdentityManager _admin = factory.AdminUser;

    [Fact]
    public async Task Success_WithNoImage()
    {
        var context = new TestContext();

        var movieRequest = new MovieRequestJsonBuilder()
            .Build();

        var request = new MovieCreateRequestApi
        {
            MovieDetails = movieRequest
        };

        context.Add(request);
        
        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var response = await _testRequest.PostMultipartForm(REQUEST_URI, request, _admin.GetAccessToken());

            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var movieId = responseData.RootElement.GetProperty("id").ToString();
            var movieTitle = responseData.RootElement.GetProperty("title").ToString();
            var movieSinopse = responseData.RootElement.GetProperty("sinopse").ToString();

            movieTitle.ShouldBe(request.MovieDetails.Title);
            movieSinopse.ShouldBe(request.MovieDetails.Sinopse);

            var dbResult = await _testRequest.DbContext.Movies.AnyAsync(m =>
                m.Id == Guid.Parse(movieId) &&
                m.Title == request.MovieDetails.Title &&
                m.Sinopse == request.MovieDetails.Sinopse &&
                m.Duration == request.MovieDetails.Duration);
            
            dbResult.ShouldBeTrue();
        });
    }

    [Fact]
    public async Task Success_WithImages()
    {
        var context = new TestContext();

        var movieRequest = new MovieRequestJsonBuilder()
            .Build();

        var request = new MovieCreateRequestApi
        {
            MovieDetails = movieRequest,
            Poster = FileFactory.GetJPEG(),
            FeaturedBanner = FileFactory.GetPNG(),
            StandardBanner = FileFactory.GetPNG()
        };

        context.Add(request);
        
        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var response = await _testRequest.PostMultipartForm(REQUEST_URI, request, _admin.GetAccessToken());

            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var movieId = responseData.RootElement.GetProperty("id").ToString();
            var movieTitle = responseData.RootElement.GetProperty("title").ToString();
            var movieSinopse = responseData.RootElement.GetProperty("sinopse").ToString();

            movieTitle.ShouldBe(request.MovieDetails.Title);
            movieSinopse.ShouldBe(request.MovieDetails.Sinopse);

            var dbResult = await _testRequest.DbContext.Movies.AnyAsync(m =>
                m.Id == Guid.Parse(movieId) &&
                m.Title == request.MovieDetails.Title &&
                m.Sinopse == request.MovieDetails.Sinopse &&
                m.Duration == request.MovieDetails.Duration);
            
            dbResult.ShouldBeTrue();
        });
    }

    [Fact]
    public async Task Validate_ShouldBeBadRequestErrorResponse_WhenImageIsTxt()
    {
        var context = new TestContext();

        var movieRequest = new MovieRequestJsonBuilder()
            .Build();

        var request = new MovieCreateRequestApi
        {
            MovieDetails = movieRequest,
            Poster = FileFactory.GetTXT()
        };

        context.Add(request);
        
        await _testLogger.ExecuteWithContext(context, async () =>
        {
            var response = await _testRequest.PostMultipartForm(REQUEST_URI, request, _admin.GetAccessToken());

            await using var responseBody = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            errors.Count().ShouldBe(1);
            errors.ShouldContain(error => 
                error.GetString().IsNotEmpty() && 
                error.GetString()!.Equals(ResourceErrorMessages.VALIDATION_INVALID_IMAGE));

            var dbResult = await _testRequest.DbContext.Movies.AnyAsync(m => 
                m.Title == request.MovieDetails.Title &&
                m.Sinopse == request.MovieDetails.Sinopse &&
                m.Duration == request.MovieDetails.Duration);
            
            dbResult.ShouldBeFalse();
        });
    }
}
