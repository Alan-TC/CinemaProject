using Moq;

namespace TestsCommonUtilities.Repositories;

public class IUnitOfWorkBuilder
{
    private readonly Mock<IUnitOfWork> _mock = new();

    public IUnitOfWorkBuilder()
    {
        _mock.Setup(x => x.Commit()).ReturnsAsync(true);
    }

    public IUnitOfWorkBuilder Commit(bool result)
    {
        _mock.Setup(x => x.Commit()).ReturnsAsync(result);
        return this;
    }

    public IUnitOfWork Build() => _mock.Object;
}
