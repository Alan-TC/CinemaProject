using CinemaBackend.Domain.Entities.UserEntities;
using CinemaBackend.Domain.Identity.Client;
using Moq;

namespace TestsCommonUtilities.Identity.Client;

public class ILoggedUserBuilder
{
    private readonly Mock<ILoggedUser> _mock = new();

    public ILoggedUserBuilder()
    {
        _mock.Setup(m => m.GetUserId()).Returns(Guid.CreateVersion7());
    }
    
    public ILoggedUser Build() => _mock.Object;
}
