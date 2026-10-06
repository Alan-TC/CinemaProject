using CinemaBackend.Communication.Requests;

namespace CinemaBackend.Client.Application.UseCases.Payments.OrderPayment;

public interface IOrderPaymentUseCase
{
    Task Execute(PaymentRequestJson request);
}
