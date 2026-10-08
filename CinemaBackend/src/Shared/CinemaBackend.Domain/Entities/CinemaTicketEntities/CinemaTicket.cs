using CinemaBackend.Domain.Entities.PaymentEntities;

namespace CinemaBackend.Domain.Entities.CinemaTicketEntities;

public class CinemaTicket : EntityBase
{
    public string Code { get; set; } = string.Empty;
    public Guid PaymentId {  get; set; }
    public Payment Payment { get; set; } = default!;
}
