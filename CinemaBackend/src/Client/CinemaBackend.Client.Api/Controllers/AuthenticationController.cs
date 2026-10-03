using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Client.Application.UseCases.Authentication.Login.WithEmailAndPassword;
using Microsoft.AspNetCore.Mvc;
using CinemaBackend.Client.Application.UseCases.Authentication.RefreshToken;

namespace CinemaBackend.Client.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(typeof(TokensResponseJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromServices] ILoginWithEmailAndPasswordUseCase useCase, [FromBody] LoginRequestJson request)
    {
        var response = await useCase.Execute(request);
        
        return Ok(response);
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(TokensResponseJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Refresh(
        [FromServices] IRefreshTokenUseCase useCase,
        [FromBody] RefreshTokenRequestJson request)
    {
        var response = await useCase.Execute(request);

        return Ok(response);
    }
}
