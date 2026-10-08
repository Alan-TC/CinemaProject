using CinemaBackend.Client.Application.UseCases.CinemaTickets.GetById;
using CinemaBackend.Client.Application.UseCases.CinemaTickets.GetUserCinemaTickets;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Communication.Responses.CinemaTickets;
using CinemaBackend.SharedKernel.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBackend.Client.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class CinemaTicketsController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(PaginationReturn<CinemaTicketResponseSummaryJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Filter(
        [FromServices] IGetUserCinemaTicketsUseCase useCase,
        [FromQuery] PaginationQueryRequest query
    )
    {
        var result = await useCase.Execute(query);

        return Ok(result);
    }

    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(CinemaTicketResponseJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(
        [FromServices] IGetUserCinemaTicketByIdUseCase useCase,
        [FromRoute] Guid id)
    {
        var result = await useCase.Execute(id);

        return Ok(result);
    }
}
