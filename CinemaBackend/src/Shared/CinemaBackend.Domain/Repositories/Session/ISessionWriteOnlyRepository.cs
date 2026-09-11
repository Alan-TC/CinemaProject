namespace CinemaBackend.Domain.Repositories.Session;

public interface ISessionWriteOnlyRepository
{
    Task Add(Entities.SessionEntities.Session session);
}
