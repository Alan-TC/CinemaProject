using CinemaBackend.Communication.Responses;

namespace CinemaBackend.Client.Application.UseCases.Payments.GetPayment;

public interface IGetPaymentUseCase
{
    Task<PaymentResponseJson> Execute(Guid id);
}
