using CinemaBackend.Admin.Application.UseCases.Session.Create;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Communication.Responses.Session;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBackend.Admin.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class SessionsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(SessionResponseJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] ISessionCreateUseCase useCase,
        [FromBody] SessionRequestJson request
    )
    {
        var result = await useCase.Execute(request);

        return Created("", result);
    }
}
