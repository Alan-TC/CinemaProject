using CinemaBackend.Domain.Repositories.Payment;
using Moq;

namespace TestsCommonUtilities.Repositories.Payment;

public class IPaymentWriteOnlyRepositoryBuilder
{
    private readonly Mock<IPaymentWriteOnlyRepository> _mock = new();
    
    public IPaymentWriteOnlyRepository Build() => _mock.Object;
}