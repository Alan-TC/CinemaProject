using CinemaBackend.Domain.Entities.PaymentEntities;
using CinemaBackend.Domain.Repositories.Payment;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data.Repositories;

internal class PaymentRepository(CinemaDbContext dbContext) : IPaymentWriteOnlyRepository, IPaymentReadOnlyRepository
{
    private readonly CinemaDbContext _dbContext = dbContext;
    
    public async Task AddPayment(Payment payment)
    {
        await _dbContext.Payments.AddAsync(payment);
    }

    public async Task<Payment?> GetById(Guid id)
    {
        return await _dbContext.Payments
            .AsNoTracking()
            .Include(e => e.Session)
            .Include(e => e.Seats)
            .FirstOrDefaultAsync(e => e.Id == id && e.DeactivatedAt == null);
    }
}