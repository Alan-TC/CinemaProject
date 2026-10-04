using CinemaBackend.Communication.Enums;
using CinemaBackend.Communication.Responses.Session;
using CinemaBackend.Domain.DTOs.Session;
using CinemaBackend.Domain.Storage.Movie;

namespace CinemaBackend.Client.Application.Mappings;

public static class SessionClientSummaryDtoMappings
{
    public static SessionClientSumaryResponseJson MapToSessionClientSumaryResponseJson(this SessionClientSummaryDto dto, IMovieImageStorage movieImageStorage)
    {
        var movieUrls = movieImageStorage.GetMovieImagesUrl(dto.MovieId);

        return new SessionClientSumaryResponseJson
        {
            Id = dto.Id,
            MovieTitle = dto.MovieTitle,
            ClassInd = (ClassInd)dto.ClassInd,
            MoviePosterUrl = movieUrls.PosterUrl,
            StartsAt = dto.StartsAt,
            EndsAt = dto.EndsAt
        };
    }
}
