namespace CinemaBackend.Domain.Entities.PaymentEntities;

public class AllocatedSeat
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string SeatCode { get; set; } = string.Empty;
    public Guid SeatId { get; set; }
    public Guid SessionId { get; set; }
    public Guid PaymentId { get; set; }
}
