namespace CinemaBackend.Domain.Repositories.Payment;

public interface IPaymentWriteOnlyRepository
{
    Task AddPayment(Entities.PaymentEntities.Payment payment);
}