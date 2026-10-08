namespace CinemaBackend.Domain.Repositories.Payment;

public interface IPaymentUpdateOnlyRepository
{
    Task<Entities.PaymentEntities.Payment?> GetById(Guid id, Guid userId);
}