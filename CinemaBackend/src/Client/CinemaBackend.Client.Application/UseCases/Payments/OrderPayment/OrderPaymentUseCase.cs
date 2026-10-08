using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.Entities.PaymentEntities;
using CinemaBackend.Domain.Enums;
using CinemaBackend.Domain.Identity.Client;
using CinemaBackend.Domain.Repositories.Payment;
using CinemaBackend.Domain.Repositories.Session;

namespace CinemaBackend.Client.Application.UseCases.Payments.OrderPayment;

public class OrderPaymentUseCase(
    IPaymentWriteOnlyRepository paymentWriteOnlyRepository,
    ISessionWriteOnlyRepository sessionWriteOnlyRepository,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork) : IOrderPaymentUseCase
{
    private readonly IPaymentWriteOnlyRepository _paymentWriteOnlyRepository = paymentWriteOnlyRepository;
    private readonly ISessionWriteOnlyRepository _sessionWriteOnlyRepository = sessionWriteOnlyRepository;

    private readonly ILoggedUser _loggedUser = loggedUser;

    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    
    public async Task<PaymentResponseJson> Execute(PaymentRequestJson request)
    {
        ValidateAndThrowOnFailures(request);
        
        var session = await _sessionWriteOnlyRepository.GetById(request.SessionId);

        if (session is null)
        {
            throw new NotFoundException();
        }

        var dateNow = DateTime.UtcNow;
        if (session.StartTicketSalesAt > dateNow || dateNow >= session.StartsAt)
        {
            throw new NotFoundException();
        }
        
        var roomFilteredSeats = session.SessionRoom.Seats
            .Where(s => request.Seats.Contains(s.Id))
            .ToList();

        if (roomFilteredSeats.Count != request.Seats.Count)
        {
            throw new NotFoundException();
        }
        
        foreach (var seat in roomFilteredSeats)
        {
            if (seat.Status == SeatStatus.Unavailable)
            {
                throw new ConflictException(ResourceErrorMessages.VALIDATION_PAYMENT_SEAT_UNAVAILABLE);
            } 
            seat.Status =  SeatStatus.Unavailable;
        }

        var allocatedSeats = roomFilteredSeats
            .Select(s => new AllocatedSeat
            {
                SeatCode = s.Code,
                SeatId = s.Id,
                SessionId = session.Id
            }).ToList();

        var totalValue = session.Pricing.NormalPrice * request.FullPriceQuantity +
                         (session.Pricing.NormalPrice / 2) * request.HalfPriceQuantity;
        
        var paymentOrder = new Payment
        {
            FullPriceQuantity = request.FullPriceQuantity,
            HalfPriceQuantity = request.HalfPriceQuantity,
            Total = totalValue,
            Seats = allocatedSeats,
            Status = PaymentStatus.Pending,
            UserId = _loggedUser.GetUserId(),
            Session = session
        };
        
        await _paymentWriteOnlyRepository.AddPayment(paymentOrder);
        
        var result = await _unitOfWork.Commit();

        if (!result)
        {
            throw new ConflictException(ResourceErrorMessages.VALIDATION_PAYMENT_SEAT_UNAVAILABLE);
        }

        return new PaymentResponseJson
        {
            Id = paymentOrder.Id,
            SessionCode = session.Code,
            FullPriceSeatsQuantity = paymentOrder.FullPriceQuantity,
            HalfPriceSeatsQuantity = paymentOrder.HalfPriceQuantity,
            SeatsCodes = paymentOrder.Seats.Select(s => s.SeatCode).ToList(),
            Total = paymentOrder.Total,
            Status = (Communication.Enums.PaymentStatus)paymentOrder.Status
        };
    }
    
    private static void ValidateAndThrowOnFailures(PaymentRequestJson request)
    {
        var validator = new PaymentRequestJsonValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            throw new ErrorOnValidationException([.. result.Errors.Select(error => error.ErrorMessage)]);
        }
    }
}
