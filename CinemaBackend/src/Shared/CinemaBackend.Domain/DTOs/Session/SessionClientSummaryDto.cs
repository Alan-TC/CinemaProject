using CinemaBackend.Domain.Enums;

namespace CinemaBackend.Domain.DTOs.Session;

public record SessionClientSummaryDto
{
    public Guid Id { get; set; }
    public Guid MovieId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public ClassInd ClassInd { get; set; }
    public DateTime StartsAt { get; set; }
    public DateTime EndsAt { get; set; }
}
