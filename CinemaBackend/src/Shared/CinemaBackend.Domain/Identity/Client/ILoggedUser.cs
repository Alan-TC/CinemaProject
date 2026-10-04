using CinemaBackend.Domain.Entities.UserEntities;

namespace CinemaBackend.Domain.Identity.Client;

public interface ILoggedUser
{
    Task<User> Get();

    Guid GetUserId();
}
