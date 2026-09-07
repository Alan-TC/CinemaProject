using CinemaBackend.Admin.Api.Requests;
using CinemaBackend.Admin.Application.UseCases.Movie.Create;
using CinemaBackend.Admin.Application.UseCases.Movie.UpdateImages;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBackend.Admin.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class MoviesController : ControllerBase
{
    [HttpPost("create")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] ICreateMovieUseCase useCase,
        [FromForm] MovieCreateRequest request)
    {
        var useCaseRequest = request.MapToCreateMovieUseCaseRequest();
        await useCase.Execute(useCaseRequest);

        return Created();
    }

    [HttpPost("{id:guid}/update/poster-image")]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePoster(
        [FromServices] IMovieUpdateImagesUseCase useCase,
        [FromRoute] Guid id,
        [FromForm] IFormFile image)
    {
        var useCaseRequest = new MovieImagesRequestJson
        {
            MovieId = id,
            Poster = image.OpenReadStream()
        };

        await useCase.Execute(useCaseRequest);

        return NoContent();
    }

    [HttpPost("{id:guid}/update/featured-banner-image")]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateFeaturedBanner(
        [FromServices] IMovieUpdateImagesUseCase useCase,
        [FromRoute] Guid id,
        [FromForm] IFormFile image)
    {
        var useCaseRequest = new MovieImagesRequestJson
        {
            MovieId = id,
            FeaturedBanner = image.OpenReadStream()
        };
        
        await useCase.Execute(useCaseRequest);

        return NoContent();
    }

    [HttpPost("{id:guid}/update/standard-banner-image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStandardBanner(
        [FromServices] IMovieUpdateImagesUseCase useCase,
        [FromRoute] Guid id,
        [FromForm] IFormFile image)
    {
        var useCaseRequest = new MovieImagesRequestJson
        {
            MovieId = id,
            StandardBanner = image.OpenReadStream()
        };
        
        await useCase.Execute(useCaseRequest);

        return NoContent();
    }
}
