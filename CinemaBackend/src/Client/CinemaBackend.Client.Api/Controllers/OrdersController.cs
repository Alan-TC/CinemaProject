using CinemaBackend.Client.Application.UseCases.Payments.GetPayment;
using CinemaBackend.Client.Application.UseCases.Payments.OrderPayment;
using CinemaBackend.Client.Application.UseCases.Payments.PayOrder;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaBackend.Client.Api.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(PaymentResponseJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrder(
        [FromServices] IGetPaymentUseCase useCase,
        [FromRoute] Guid id)
    {
        var result = await useCase.Execute(id);

        return Ok(result);
    }

    [HttpGet("{id:Guid}/pay")]
    [ProducesResponseType(typeof(PaymentResponseJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PayOrder(
        [FromServices] IPayOrderUseCase useCase,
        [FromRoute] Guid id)
    {
        await useCase.Execute(id);

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(typeof(PaymentResponseJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ErrorResponseJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Order(
        [FromServices] IOrderPaymentUseCase useCase,
        [FromBody] PaymentRequestJson request)
    {
        var result = await useCase.Execute(request);

        return Created("", result);
    }
}
