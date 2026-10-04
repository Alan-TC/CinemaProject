using CinemaBackend.Client.Application.UseCases.Sessions.GetById;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Communication.Responses.Session;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBackend.Client.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class SessionsController : ControllerBase
{
    [HttpGet("{sessionId:guid}")]
    [ProducesResponseType(typeof(SessionResponseJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromServices] ISessionGetByIdUseCase useCase,
        [FromRoute] Guid sessionId
    )
    {
        var result = await useCase.Execute(sessionId);

        return Ok(result);
    }
}
