using CinemaBackend.Admin.Application.Requests;
using CinemaBackend.Admin.Application.Responses;
using CinemaBackend.Admin.Application.UseCases.Authentication.Login.WithEmailAndPassword;
using CinemaBackend.Domain.Responses;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace;

[Route("[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(ResponseTokensJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] ILoginWithEmailAndPasswordUseCase useCase, [FromBody] LoginRequestJson request)
    {
        var response = await useCase.Execute(request);
        
        return Ok(response);
    }
}
