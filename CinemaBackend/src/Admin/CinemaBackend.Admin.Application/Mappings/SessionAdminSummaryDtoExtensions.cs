using CinemaBackend.Communication.Responses.Session;
using CinemaBackend.Domain.DTOs.Session;

namespace CinemaBackend.Admin.Application.Mappings;

public static class SessionAdminSummaryDtoExtensions
{
    public static SessionAdminSumaryResponseJson MapToSessionAdminSumaryResponseJson(this SessionAdminSummaryDto dto)
    {
        return new SessionAdminSumaryResponseJson
        {
            Id = dto.Id,
            Code = dto.Code,
            MovieTitle = dto.MovieTitle,
            StartsAt = dto.StartsAt,
            EndsAt = dto.EndsAt,
            StartTicketSalesAt = dto.StartTicketSalesAt,
            CreatedAt = dto.CreatedAt
        };
    }
}
