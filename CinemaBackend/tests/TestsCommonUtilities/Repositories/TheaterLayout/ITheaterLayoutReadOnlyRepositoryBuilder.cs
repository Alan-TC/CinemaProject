using CinemaBackend.Domain.Repositories.TheaterLayout;
using Moq;

namespace TestsCommonUtilities.Repositories.TheaterLayout;

public class ITheaterLayoutReadOnlyRepositoryBuilder
{
    private readonly Mock<ITheaterLayoutReadOnlyRepository> _mock = new();

    public ITheaterLayoutReadOnlyRepositoryBuilder HasTheaterWithTag(string tag)
    {
        _mock.Setup(r => r.HasTheaterWithTag(tag)).ReturnsAsync(true);
        return this;
    }

    public ITheaterLayoutReadOnlyRepositoryBuilder GetById(CinemaBackend.Domain.Entities.TheaterLayoutEntities.TheaterLayout theater)
    {
        _mock.Setup(r => r.GetById(theater.Id)).ReturnsAsync(theater);
        return this;
    }
    public ITheaterLayoutReadOnlyRepository Build() => _mock.Object;
}
