using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Domain.Entities.CinemaTicketEntities;
using CinemaBackend.Domain.Enums;
using CinemaBackend.Domain.Identity.Client;
using CinemaBackend.Domain.Repositories.CinemaTicket;
using CinemaBackend.Domain.Repositories.Payment;
using CinemaBackend.SharedKernel.Services;

namespace CinemaBackend.Client.Application.UseCases.Payments.PayOrder;

public class PayOrderUseCase(
    IPaymentUpdateOnlyRepository paymentUpdateOnlyRepository,
    ICinemaTicketReadOnlyRepository cinemaTicketReadOnlyRepository,
    ICinemaTicketWriteOnlyRepository cinemaTicketWriteOnlyRepository,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork) : IPayOrderUseCase
{
    private readonly IPaymentUpdateOnlyRepository _paymentUpdateOnlyRepository = paymentUpdateOnlyRepository;
    private readonly ICinemaTicketReadOnlyRepository _cinemaTicketReadOnlyRepository = cinemaTicketReadOnlyRepository;
    private readonly ICinemaTicketWriteOnlyRepository _cinemaTicketWriteOnlyRepository = cinemaTicketWriteOnlyRepository;

    private readonly ILoggedUser _loggedUser = loggedUser;

    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(Guid id)
    {
        var payment = await _paymentUpdateOnlyRepository.GetById(id, _loggedUser.GetUserId());

        if (payment == null)
        {
            throw new NotFoundException();
        }
        if (payment.Status == PaymentStatus.Paid)
        {
            return;
        }

        payment.Status = PaymentStatus.Paid;

        var ticketCode = await GenerateValidCode();

        var cinemaTicket = new CinemaTicket
        {
            Code = ticketCode,
            Payment = payment
        };

        await _cinemaTicketWriteOnlyRepository.AddTicket(cinemaTicket);

        _ = _unitOfWork.Commit();
    }

    public async Task<string> GenerateValidCode()
    {
        var code = "";

        do
        {
            code = CodeGenerator.GenerateCode(8);
        } while (await _cinemaTicketReadOnlyRepository.HasTicketWithCode(code));

        return code;
    }
}
