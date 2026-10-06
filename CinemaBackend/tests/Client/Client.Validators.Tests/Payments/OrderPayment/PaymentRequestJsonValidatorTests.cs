using CinemaBackend.Client.Application.UseCases.Payments.OrderPayment;
using CinemaBackend.Communication.ResourceFiles.Errors;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Requests;
using Xunit.Abstractions;

namespace Client.Validators.Tests.Payments.OrderPayment;

public class PaymentRequestJsonValidatorTests(ITestOutputHelper output)
{
    private readonly TestLoggingClass _testLogger = new(output);

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        var request = new PaymentRequestJsonBuilder()
            .Build();
        context.Add(request);

        var validator = new PaymentRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeTrue();
            result.Errors.ShouldBeEmpty();
        });        
    }

    [Fact]
    public async Task Validate_ShouldHaveAnError_WhenSeatsIsEmpty()
    {
        var context = new TestContext();

        var request = new PaymentRequestJsonBuilder()
            .WithFullPriceQuantity(0)
            .WithHalfPriceQuantity(0)
            .WithTotalSeats(0)
            .Build();
        context.Add(request);

        var validator = new PaymentRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_PAYMENT_ORDER_SEAT_QTD_INVALID));
        });
    }

    [Theory]
    [InlineData(1, 2, 2)]
    [InlineData(2, 1, 4)]
    [InlineData(0, 2, 4)]
    [InlineData(2, 0, 4)]
    [InlineData(5, 1, 5)]
    public async Task Validate_ShouldHaveAnError_WhenTotalSumIsInvalid(int fullQuantity, int halfQuantity, int totalSeats)
    {
        var context = new TestContext();

        var request = new PaymentRequestJsonBuilder()
            .WithFullPriceQuantity(fullQuantity)
            .WithHalfPriceQuantity(halfQuantity)
            .WithTotalSeats(totalSeats)
            .Build();
        context.Add(request);

        var validator = new PaymentRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_PAYMENT_ORDER_TOTAL_SEAT_SUM_INVALID)); 
        });
    }
    
    [Fact]
    public async Task Validate_ShouldHaveAnError_WhenFullPriceQtdIsLessThanZero()
    {
        var context = new TestContext();

        var request = new PaymentRequestJsonBuilder()
            .WithFullPriceQuantity(-1)
            .WithHalfPriceQuantity(2)
            .WithTotalSeats(1)
            .Build();
        context.Add(request);

        var validator = new PaymentRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_PAYMENT_ORDER_FULL_PRICE_QTD_INVALID)); 
        });
    }
    
    [Fact]
    public async Task Validate_ShouldHaveAnError_WhenHalfPriceQtdIsLessThanZero()
    {
        var context = new TestContext();

        var request = new PaymentRequestJsonBuilder()
            .WithFullPriceQuantity(2)
            .WithHalfPriceQuantity(-1)
            .WithTotalSeats(1)
            .Build();
        context.Add(request);

        var validator = new PaymentRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_PAYMENT_ORDER_HALF_PRICE_QTD_INVALID)); 
        });
    }
    
    [Theory]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(10)]
    [InlineData(20)]
    public async Task Validate_ShouldHaveAnError_WhenTotalSeatsIsGreaterThan5(int totalSeats)
    {
        var context = new TestContext();

        var request = new PaymentRequestJsonBuilder()
            .WithFullPriceQuantity(totalSeats)
            .WithHalfPriceQuantity(0)
            .WithTotalSeats(totalSeats)
            .Build();
        context.Add(request);

        var validator = new PaymentRequestJsonValidator();

        _testLogger.ExecuteWithContext(context, () =>
        {
            var result = validator.Validate(request);

            result.IsValid.ShouldBeFalse();
            result.Errors.Count.ShouldBe(1);
            result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceErrorMessages.VALIDATION_PAYMENT_ORDER_SEAT_MAX_QTD_INVALID)); 
        });
    }
}
