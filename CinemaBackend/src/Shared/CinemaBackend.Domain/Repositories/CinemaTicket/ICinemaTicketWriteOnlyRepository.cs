namespace CinemaBackend.Domain.Repositories.CinemaTicket;

public interface ICinemaTicketWriteOnlyRepository
{
    Task AddTicket(Entities.CinemaTicketEntities.CinemaTicket ticket);
}
