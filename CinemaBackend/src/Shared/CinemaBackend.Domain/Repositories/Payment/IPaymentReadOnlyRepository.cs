namespace CinemaBackend.Domain.Repositories.Payment;

public interface IPaymentReadOnlyRepository
{
    Task<Entities.PaymentEntities.Payment?> GetById(Guid id);
}