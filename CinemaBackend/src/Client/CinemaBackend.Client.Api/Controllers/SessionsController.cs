using CinemaBackend.Client.Application.UseCases.Sessions.GetById;
using CinemaBackend.Client.Application.UseCases.Sessions.GetSessions;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Communication.Responses.Session;
using CinemaBackend.SharedKernel.Pagination;
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

    [HttpPost("filter")]
    [ProducesResponseType(typeof(PaginationReturn<SessionClientSumaryResponseJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Filter(
    [FromServices] IGetSessionsFilteredPagedUseCase useCase,
    [FromQuery] PaginationQueryRequest query,
    [FromBody] SessionClientFilterJson? filter
    )
    {
        var result = await useCase.Execute(query, filter?.SearchTerm);

        return Ok(result);
    }
}
