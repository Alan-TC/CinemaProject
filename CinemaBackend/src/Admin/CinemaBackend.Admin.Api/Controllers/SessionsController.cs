using CinemaBackend.Admin.Application.UseCases.Session;
using CinemaBackend.Admin.Application.UseCases.Session.Create;
using CinemaBackend.Admin.Application.UseCases.Session.GetSessions;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Communication.Responses.Session;
using CinemaBackend.SharedKernel.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBackend.Admin.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
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

    [HttpPost("filter")]
    [ProducesResponseType(typeof(PaginationReturn<SessionAdminSumaryResponseJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Filter(
        [FromServices] IGetSessionsFilteredPagedUseCase useCase,
        [FromQuery] PaginationQueryRequest query,
        [FromBody] SessionAdminFilterJson? filter
    )
    {
        var result = await useCase.Execute(query, filter?.Code);

        return Ok(result);
    }

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
