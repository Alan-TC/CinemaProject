using CinemaBackend.Admin.Application.UseCases.Session.Create;
using CinemaBackend.Communication.ResourceFiles.Errors;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Requests;
using Xunit.Abstractions;

namespace Admin.Validators.Tests.Session.Create;

public class SessionRequestJsonValidatorTests(ITestOutputHelper output)
{
    private readonly TestLoggingClass _testLogger = new(output);

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        var request = new SessionRequestJsonBuilder()
            .Build();

        context.Add(request);

        var validator = new SessionRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
            result.Errors.ShouldBeEmpty();
        });
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-0.2)]
    [InlineData(-10)]
    public async Task Validade_ShouldBeAnError_WhenPriceIsLessThanOrEqualToZero(decimal price)
    {
        var context = new TestContext();

        var request = new SessionRequestJsonBuilder()
            .WithPrice(price)
            .Build();

        context.Add(request);

        var validator = new SessionRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_SESSION_INVALID_PRICE));
        });
    }

    [Fact]
    public async Task Validade_ShouldBeAnError_WhenSessionStartsBeforeTicketsSales()
    {
        var context = new TestContext();

        var ticketSalesAt = DateTime.UtcNow;
        var startsAt = ticketSalesAt.AddDays(new Random().Next(1, 20));
        var endsAt = startsAt.AddMinutes(new Random().Next(55, 240));

        startsAt = ticketSalesAt.AddDays(-new Random().Next(1, 20));

        var request = new SessionRequestJsonBuilder()
            .WithDates(ticketSalesAt, startsAt, endsAt)
            .Build();

        context.Add(request);

        var validator = new SessionRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_SESSION_INVALID_TICKET_SALES_TIME));
        });
    }

    [Fact]
    public async Task Validade_ShouldBeAnError_WhenSessionEndsBeforeStarts()
    {
        var context = new TestContext();

        var ticketSalesAt = DateTime.UtcNow;
        var startsAt = ticketSalesAt.AddDays(new Random().Next(1, 20));
        var endsAt = startsAt.AddMinutes(new Random().Next(55, 240));

        endsAt = startsAt.AddMinutes(-new Random().Next(55, 240));

        var request = new SessionRequestJsonBuilder()
            .WithDates(ticketSalesAt, startsAt, endsAt)
            .Build();

        context.Add(request);

        var validator = new SessionRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_SESSION_INVALID_END_TIME));
        });
    }
}
