using CinemaBackend.Communication.Enums;

namespace CinemaBackend.Communication.Responses;

public class PaymentResponseJson
{
    public Guid Id { get; set; }
    public string SessionCode  { get; set; } = string.Empty;
    public int FullPriceSeatsQuantity { get; set; }
    public int HalfPriceSeatsQuantity { get; set; }
    public decimal Total { get; set; }
    public PaymentStatus Status { get; set; }
    public ICollection<string> SeatsCodes { get; set; } = [];
}