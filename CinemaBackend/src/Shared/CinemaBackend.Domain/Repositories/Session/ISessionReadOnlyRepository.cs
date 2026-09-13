namespace CinemaBackend.Domain.Repositories.Session;

public interface ISessionReadOnlyRepository
{
    Task<bool> HasSessionWithCode(string code);

    Task<Entities.SessionEntities.Session?> GetByCode(string code);

    Task<Entities.SessionEntities.Session?> GetById(Guid id);
}
