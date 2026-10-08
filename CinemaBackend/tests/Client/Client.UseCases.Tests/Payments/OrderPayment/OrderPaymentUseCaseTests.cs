using System.Net;
using CinemaBackend.Client.Application.UseCases.Payments.OrderPayment;
using CinemaBackend.Communication.Exceptions;
using CinemaBackend.Communication.ResourceFiles.Errors;
using CinemaBackend.Domain.Entities.SessionEntities;
using CinemaBackend.Domain.Enums;
using Shouldly;
using TestsCommonUtilities;
using TestsCommonUtilities.Entities;
using TestsCommonUtilities.Identity.Client;
using TestsCommonUtilities.Repositories;
using TestsCommonUtilities.Repositories.Payment;
using TestsCommonUtilities.Repositories.Sessions;
using TestsCommonUtilities.Requests;
using Xunit.Abstractions;

namespace Client.UseCases.Tests.Payments.OrderPayment;

public class OrderPaymentUseCaseTests(ITestOutputHelper output)
{
    private readonly TestLoggingClass _testLogger = new(output);

    [Fact]
    public async Task Success()
    {
        var context = new TestContext();

        var session = new SessionBuilder()
            .WithDateUtcNow()
            .Build();
        context.Add(session);
        
        var request = new PaymentRequestJsonBuilder()
            .WithSessionId(session.Id)
            .WithSeats(session.SessionRoom.Seats.Take(5).Select(s => s.Id).ToList())
            .Build();
        context.Add(request);

        var useCase = CreateUseCase(session);

        await _testLogger.ExecuteWithContext(request, async () =>
        {
           await useCase.Execute(request).ShouldNotThrowAsync(); 
        });
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenSessionNotFound()
    {
        var context = new TestContext();

        var request = new PaymentRequestJsonBuilder()
            .Build();
        context.Add(request);

        var useCase = CreateUseCase();

        await _testLogger.ExecuteWithContext(context, async() =>
        {
            var result = await useCase.Execute(request).ShouldThrowAsync<NotFoundException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.NotFound);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(message => message.Equals(ResourceErrorMessages.RESOURCE_NOT_FOUND));
        });
    }
    
    [Fact]
    public async Task Validate_ShouldThrowException_WhenSeatsNotFound()
    {
        var context = new TestContext();
        
        var session = new SessionBuilder()
            .WithDateUtcNow()
            .Build();

        var request = new PaymentRequestJsonBuilder()
            .WithSessionId(session.Id)
            .Build();
        context.Add(request);

        var useCase = CreateUseCase(session);

        await _testLogger.ExecuteWithContext(context, async() =>
        {
            var result = await useCase.Execute(request).ShouldThrowAsync<NotFoundException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.NotFound);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(message => message.Equals(ResourceErrorMessages.RESOURCE_NOT_FOUND));
        });
    }
    
    [Fact]
    public async Task Validate_ShouldThrowException_WhenSeatUnavailable()
    {
        var context = new TestContext();
        
        var session = new SessionBuilder()
            .WithDateUtcNow()
            .Build();

        var seat = session.SessionRoom.Seats.First();
        seat.Status = SeatStatus.Unavailable;

        var request = new PaymentRequestJsonBuilder()
            .WithSessionId(session.Id)
            .WithSeats([seat.Id])
            .Build();
        context.Add(request);

        var useCase = CreateUseCase(session);

        await _testLogger.ExecuteWithContext(context, async() =>
        {
            var result = await useCase.Execute(request).ShouldThrowAsync<ConflictException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.Conflict);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(message => message.Equals(ResourceErrorMessages.VALIDATION_PAYMENT_SEAT_UNAVAILABLE));
        });
    }
    
    [Fact]
    public async Task Validate_ShouldThrowException_WhenSeatUnavailableOnConcurrency()
    {
        var context = new TestContext();
        
        var session = new SessionBuilder()
            .WithDateUtcNow()
            .Build();

        var seat = session.SessionRoom.Seats.First();

        var request = new PaymentRequestJsonBuilder()
            .WithSessionId(session.Id)
            .WithSeats([seat.Id])
            .Build();
        context.Add(request);

        var useCase = CreateUseCase(session, true);

        await _testLogger.ExecuteWithContext(context, async() =>
        {
            var result = await useCase.Execute(request).ShouldThrowAsync<ConflictException>();

            result.GetErrorCode().ShouldBe(HttpStatusCode.Conflict);
            result.GetErrorMessages().Count.ShouldBe(1);
            result.GetErrorMessages().ShouldContain(message => message.Equals(ResourceErrorMessages.VALIDATION_PAYMENT_SEAT_UNAVAILABLE));
        });
    }

    private static OrderPaymentUseCase CreateUseCase(Session? session = null, bool concurrencyError = false)
    {
        var sessionWriteOnlyRepositoryBuilder = new ISessionWriteOnlyRepositoryBuilder();
        var paymentWriteOnlyRepository = new IPaymentWriteOnlyRepositoryBuilder().Build();
        var loggedUser = new ILoggedUserBuilder().Build();
        var unitOfWork = new IUnitOfWorkBuilder().Commit(!concurrencyError).Build();
        
        if (session is not null)
        {
            sessionWriteOnlyRepositoryBuilder.GetById(session);
        }

        return new OrderPaymentUseCase
        (
            loggedUser: loggedUser,
            paymentWriteOnlyRepository: paymentWriteOnlyRepository,
            sessionWriteOnlyRepository: sessionWriteOnlyRepositoryBuilder.Build(),
            unitOfWork: unitOfWork
        );
    }
}
