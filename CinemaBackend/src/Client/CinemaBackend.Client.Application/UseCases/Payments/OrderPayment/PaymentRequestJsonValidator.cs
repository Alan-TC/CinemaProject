using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.ResourceFiles.Errors;
using FluentValidation;

namespace CinemaBackend.Client.Application.UseCases.Payments.OrderPayment;

public class PaymentRequestJsonValidator : AbstractValidator<PaymentRequestJson>
{
    public PaymentRequestJsonValidator()
    {
        RuleFor(r => r.FullPriceQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ResourceErrorMessages.VALIDATION_PAYMENT_ORDER_FULL_PRICE_QTD_INVALID);
        
        RuleFor(r => r.HalfPriceQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ResourceErrorMessages.VALIDATION_PAYMENT_ORDER_HALF_PRICE_QTD_INVALID);

        RuleFor(r => r.Seats)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.VALIDATION_PAYMENT_ORDER_SEAT_QTD_INVALID);

        RuleFor(r => r.Seats.Count)
            .LessThanOrEqualTo(5)
            .WithMessage(ResourceErrorMessages.VALIDATION_PAYMENT_ORDER_SEAT_MAX_QTD_INVALID);
        
        RuleFor(r => r.Seats.Count)
            .Equal(r => r.FullPriceQuantity + r.HalfPriceQuantity)
            .WithMessage(ResourceErrorMessages.VALIDATION_PAYMENT_ORDER_TOTAL_SEAT_SUM_INVALID);
    }
}
