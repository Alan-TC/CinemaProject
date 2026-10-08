using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.Identity.Client;
using CinemaBackend.Domain.Repositories.Payment;

namespace CinemaBackend.Client.Application.UseCases.Payments.GetPayment;

public class GetPaymentUseCase(
    IPaymentReadOnlyRepository paymentReadOnlyRepository,
    ILoggedUser loggedUser) : IGetPaymentUseCase
{
    private readonly IPaymentReadOnlyRepository _paymentReadOnlyRepository = paymentReadOnlyRepository;

    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task<PaymentResponseJson> Execute(Guid id)
    {
        var payment = await _paymentReadOnlyRepository.GetById(id, _loggedUser.GetUserId());

        if (payment == null)
        {
            throw new NotFoundException();
        }

        return new PaymentResponseJson
        {
            Id = payment.Id,
            SessionCode = payment.Session.Code,
            FullPriceSeatsQuantity = payment.FullPriceQuantity,
            HalfPriceSeatsQuantity = payment.HalfPriceQuantity,
            SeatsCodes = payment.Seats.Select(s => s.SeatCode).ToList(),
            Total = payment.Total,
            Status = (Communication.Enums.PaymentStatus)payment.Status
        };
    }
}
