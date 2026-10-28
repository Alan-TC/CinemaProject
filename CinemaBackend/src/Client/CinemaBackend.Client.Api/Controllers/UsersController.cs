using CinemaBackend.Client.Application.UseCases.Users.GetProfile;
using CinemaBackend.Client.Application.UseCases.Users.Register;
using CinemaBackend.Client.Application.UseCases.Users.UpdateProfile;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBackend.Client.Api.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] UserRequestJson request)
    {
        await useCase.Execute(request);

        return Created();
    }

    [Authorize]
    [HttpGet("profile")]
    [ProducesResponseType(typeof(UserProfileJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromServices] IGetUserProfileUseCase useCase)
    {
        var result = await useCase.Execute();

        return Ok(result);
    }

    [Authorize]
    [HttpPut("profile")]
    [ProducesResponseType(typeof(UserProfileJson), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateUserProfileUseCase useCase,
        [FromBody] UserProfileRequestJson request)
    {
        await useCase.Execute(request);

        return Ok();
    }
}
