using Moq;

namespace TestsCommonUtilities.Repositories;

public class IUnitOfWorkBuilder
{
    private readonly Mock<IUnitOfWork> _mock = new();

    public IUnitOfWork Build() => _mock.Object;
}
