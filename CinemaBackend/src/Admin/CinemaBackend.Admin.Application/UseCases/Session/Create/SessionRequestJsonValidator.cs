using CinemaBackend.Communication.Requests;
using CinemaBackend.Communication.ResourceFiles.Errors;
using FluentValidation;

namespace CinemaBackend.Admin.Application.UseCases.Session.Create;

public class SessionRequestJsonValidator : AbstractValidator<SessionRequestJson>
{
    public SessionRequestJsonValidator()
    {
        RuleFor(r => r.StartsAt)
            .LessThan(r => r.EndsAt)
            .WithMessage(ResourceErrorMessages.VALIDATION_SESSION_INVALID_END_TIME);

        RuleFor(r => r.StartTicketSalesAt)
            .LessThan(r => r.StartsAt)
            .WithMessage(ResourceErrorMessages.VALIDATION_SESSION_INVALID_TICKET_SALES_TIME);

        RuleFor(r => r.Pricing)
            .GreaterThan(0)
            .WithMessage(ResourceErrorMessages.VALIDATION_SESSION_INVALID_PRICE);
    }
}
