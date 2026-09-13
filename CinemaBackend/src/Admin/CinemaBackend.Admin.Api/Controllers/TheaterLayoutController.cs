using CinemaBackend.Admin.Application.UseCases.TheaterLayout.Create;
using CinemaBackend.Admin.Application.UseCases.TheaterLayout.GetById;
using CinemaBackend.Admin.Application.UseCases.TheaterLayout.GetSelector;
using CinemaBackend.Admin.Application.UseCases.TheaterLayout.GetTheaterLayouts;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using CinemaBackend.SharedKernel.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBackend.Admin.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class TheaterLayoutController : ControllerBase
{

    [HttpGet("{theaterId:guid}")]
    [ProducesResponseType(typeof(TheaterLayoutResponseJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromServices] IGetTheaterLayoutByIdUseCase useCase,
        [FromRoute] Guid theaterId
    )
    {
        var result = await useCase.Execute(theaterId);

        return Ok(result);
    }

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

    [HttpPost("filter")]
    [ProducesResponseType(typeof(PaginationReturn<TheaterLayoutResponseJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Create(
        [FromServices] IGetFilteredTheaterLayoutsUseCase useCase,
        [FromQuery] PaginationQueryRequest query,
        [FromBody] TheaterLayoutFilterRequestJson? filter
    )
    {
        var result = await useCase.Execute(query, filter);

        return Ok(result);
    }

    [HttpPost("selector")]
    [ProducesResponseType(typeof(PaginationReturn<TheaterLayoutResponseJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Selector(
        [FromServices] IGetTheaterLayoutSelectorUseCase useCase,
        [FromQuery] PaginationQueryRequest query,
        [FromBody] TheaterSelectorFilterRequestJson? filter
    )
    {
        var result = await useCase.Execute(query, filter);

        return Ok(result);
    }
}
