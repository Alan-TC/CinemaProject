using CinemaBackend.Client.Application.UseCases.Movies.GetById;
using CinemaBackend.Client.Application.UseCases.Movies.GetMovies;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.SharedKernel.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBackend.Client.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    [HttpGet("{movieId:guid}")]
    [ProducesResponseType(typeof(MovieResponseJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromServices] IMovieGetByIdUseCase useCase,
        [FromRoute] Guid movieId)
    {
        var result = await useCase.Execute(movieId);

        return Ok(result);
    }

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
}
