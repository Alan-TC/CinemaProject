namespace CinemaBackend.Domain.Repositories.TheaterLayout;

public interface ITheaterLayoutWriteOnlyRepository
{
    Task Add(Entities.TheaterLayoutEntities.TheaterLayout theaterLayout);
}
