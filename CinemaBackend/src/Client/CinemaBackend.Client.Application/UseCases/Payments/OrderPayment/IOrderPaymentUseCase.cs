using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.Responses;

namespace CinemaBackend.Client.Application.UseCases.Payments.OrderPayment;

public interface IOrderPaymentUseCase
{
    Task<PaymentResponseJson> Execute(PaymentRequestJson request);
}
