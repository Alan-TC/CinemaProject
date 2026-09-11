using CinemaBackend.Admin.Application.UseCases.Session.Create;
using CinemaBackend.Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBackend.Admin.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class SessionsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromServices] ISessionCreateUseCase useCase,
        [FromBody] SessionRequestJson request
    )
    {
        var result = await useCase.Execute(request);

        return Created("", result);
    }
}
