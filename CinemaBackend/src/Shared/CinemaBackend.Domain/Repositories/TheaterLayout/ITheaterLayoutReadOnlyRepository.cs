namespace CinemaBackend.Domain.Repositories.TheaterLayout;

public interface ITheaterLayoutReadOnlyRepository
{
    Task<bool> HasTheaterWithTag(string tag);
}
