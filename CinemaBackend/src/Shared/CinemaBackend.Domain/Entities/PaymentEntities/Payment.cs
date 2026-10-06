using CinemaBackend.Domain.Entities.SessionEntities;
using CinemaBackend.Domain.Enums;

namespace CinemaBackend.Domain.Entities.PaymentEntities;

public class Payment : EntityBase
{
    public int FullPriceQuantity { get; set; }
    public int HalfPriceQuantity { get; set; }
    public decimal Total {  get; set; }
    public ICollection<AllocatedSeat> Seats { get; set; } = [];
    public PaymentStatus Status { get; set; }
    public Guid SessionId { get; set; }
    public Session Session { get; set; } = default!;
}
