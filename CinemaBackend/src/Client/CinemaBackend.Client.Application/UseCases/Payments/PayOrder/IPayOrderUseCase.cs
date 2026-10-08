using CinemaBackend.Communication.Responses;

namespace CinemaBackend.Client.Application.UseCases.Payments.PayOrder;

public interface IPayOrderUseCase
{
    Task Execute(Guid id);
}
