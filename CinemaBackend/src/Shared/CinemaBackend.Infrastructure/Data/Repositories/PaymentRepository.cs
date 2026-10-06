using CinemaBackend.Domain.Entities.PaymentEntities;
using CinemaBackend.Domain.Repositories.Payment;

namespace CinemaBackend.Infrastructure.Data.Repositories;

internal class PaymentRepository(CinemaDbContext dbContext) : IPaymentWriteOnlyRepository
{
    private readonly CinemaDbContext _dbContext = dbContext;
    
    public async Task AddPayment(Payment payment)
    {
        await _dbContext.Payments.AddAsync(payment);
    }
}