namespace CinemaBackend.Communication.Requests;

public class PaymentRequestJson
{
    public Guid SessionId { get; set; }
    public int FullPriceQuantity { get; set; }
    public int HalfPriceQuantity { get; set; }
    public ICollection<Guid> Seats { get; set; } = [];
}
