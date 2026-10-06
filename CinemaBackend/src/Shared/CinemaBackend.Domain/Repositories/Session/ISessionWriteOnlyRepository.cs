namespace CinemaBackend.Domain.Repositories.Session;

public interface ISessionWriteOnlyRepository
{
    Task<Entities.SessionEntities.Session?> GetById(Guid  sessionId);
    Task Add(Entities.SessionEntities.Session session);
}
