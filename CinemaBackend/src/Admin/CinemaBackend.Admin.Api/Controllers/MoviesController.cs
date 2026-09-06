using CinemaBackend.Admin.Api.Requests;
using CinemaBackend.Admin.Application.UseCases.Movie.Create;
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
}
