using CinemaBackend.Client.Application.UseCases.Payments.OrderPayment;
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
