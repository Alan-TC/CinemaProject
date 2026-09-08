using CinemaBackend.Admin.Application.UseCases.TheaterLayout.Create;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBackend.Admin.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class TheaterLayoutController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(TheaterLayoutResponseJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create(
        [FromServices] ICreateTheaterLayoutUseCase useCase,
        [FromBody] TheaterLayoutRequestJson request
    )
    {
        var result = await useCase.Execute(request);

        return Created("", result);
    }
}
