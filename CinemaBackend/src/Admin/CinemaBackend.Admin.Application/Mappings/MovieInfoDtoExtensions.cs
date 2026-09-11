using CinemaBackend.Communication.Responses;
using CinemaBackend.Domain.DTOs.Movie;

namespace CinemaBackend.Admin.Application.Mappings;

internal static class MovieInfoDtoExtensions
{
    internal static MovieResponseJson MapToMovieResponseJson(this MovieInfoDto dto)
    {
        return new MovieResponseJson
        {
            Id = dto.Id,
            Title = dto.Title,
            Sinopse = dto.Sinopse,
            Duration = dto.Duration,
            ReleaseDate = dto.ReleaseDate,
            ClassInd = (Communication.Enums.ClassInd)dto.ClassInd,
            FilmGenres = [.. dto.FilmGenres.Select(g => (Communication.Enums.Genres)g)],
            ImagesUrl = dto.ImagesUrl is null ? new MovieImagesUrlResponseJson() : new MovieImagesUrlResponseJson
            {
                PosterUrl = dto.ImagesUrl.PosterUrl,
                FeaturedBanner = dto.ImagesUrl.FeaturedBanner,
                StandardBanner = dto.ImagesUrl.StandardBanner,
            }
        };
    }
}
