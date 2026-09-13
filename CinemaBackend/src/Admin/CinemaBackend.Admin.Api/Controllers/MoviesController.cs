using CinemaBackend.Admin.Api.Requests;
using CinemaBackend.Admin.Application.UseCases.Movie.Create;
using CinemaBackend.Admin.Application.UseCases.Movie.GetById;
using CinemaBackend.Admin.Application.UseCases.Movie.GetMovies;
using CinemaBackend.Admin.Application.UseCases.Movie.GetMovieSelector;
using CinemaBackend.Admin.Application.UseCases.Movie.UpdateDetails;
using CinemaBackend.Admin.Application.UseCases.Movie.UpdateImages;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.DTOs.Movie;
using CinemaBackend.SharedKernel.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBackend.Admin.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class MoviesController : ControllerBase
{

    [HttpPost("filter")]
    [ProducesResponseType(typeof(PaginationReturn<MovieResponseJson>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(
        [FromServices] IMoviesGetFilteredPagedUseCase useCase,
        [FromQuery] PaginationQueryRequest query,
        [FromBody] MovieFilterRequestJson? filter)
    {
        var result = await useCase.Execute(query, filter);

        return Ok(result);
    }

    [HttpPost("selector")]
    [ProducesResponseType(typeof(PaginationReturn<MovieSelectorItemResponseJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Selector(
        [FromServices] IGetMovieSelectorUseCase useCase,
        [FromQuery] PaginationQueryRequest query,
        [FromBody] MovieSelectorRequestJson? filter)
    {
        var result = await useCase.Execute(query, filter);

        return Ok(result);
    }

    [HttpGet("{movieId:guid}")]
    [ProducesResponseType(typeof(MovieInfoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(
        [FromServices] IMovieGetByIdUseCase useCase,
        [FromRoute] Guid movieId)
    {
        var result = await useCase.Execute(movieId);

        return Ok(result);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(MovieResponseJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] ICreateMovieUseCase useCase,
        [FromForm] MovieCreateRequest request)
    {
        var useCaseRequest = request.MapToCreateMovieUseCaseRequest();
        var result = await useCase.Execute(useCaseRequest);

        return Created("", result);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateMovie(
        [FromServices] IUpdateMovieDetailsUseCase useCase,
        [FromRoute] Guid id,
        [FromBody] MovieRequestJson request)
    {
        await useCase.Execute(id, request);

        return NoContent();
    }

    [HttpPut("{id:guid}/poster-image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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

    [HttpPut("{id:guid}/featured-banner-image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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

    [HttpPut("{id:guid}/standard-banner-image")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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
