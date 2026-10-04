using CinemaBackend.Domain.DTOs.Movie;
using CinemaBackend.Domain.Entities.MovieEntities;
using CinemaBackend.Domain.Repositories.Movie;
using CinemaBackend.SharedKernel.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data.Repositories;

internal class MovieRepository(CinemaDbContext dbContext) : IMovieWriteOnlyRepository, IMovieReadOnlyRepository, IMovieUpdateOnlyRepository
{
    private readonly CinemaDbContext _dbContext = dbContext;

    public async Task Add(Movie movie) => await _dbContext.Movies.AddAsync(movie);

    public async Task<PaginationReturn<MovieInfoDto>> GetFilteredPagedMovies(PaginationQuery query, MovieFilterDto filter, bool onlyInActiveSessions = false)
    {
        var filteredQuery = _dbContext.Movies
            .AsNoTracking()
            .Where(m => m.DeactivatedAt == null);
        
        if (filter.ClassInd != null)
            filteredQuery = filteredQuery.Where(m => m.ClassInd == filter.ClassInd);

        if (filter.ReleaseDate != null)
            filteredQuery = filteredQuery
                .Where(m => m.ReleaseDate >= filter.ReleaseDate.From && 
                    m.ReleaseDate <= filter.ReleaseDate.To);

        if(onlyInActiveSessions)
        {
            var dateNow = DateTime.UtcNow;
            filteredQuery = filteredQuery
                .Where(m => _dbContext.Sessions.Any(s => s.MovieId == m.Id && s.StartTicketSalesAt <= dateNow && dateNow < s.StartsAt));
        }

        if (filter.SearchTerm != null)
        {
            var searchTerms = filter.SearchTerm.ToLower().Split(", ").Select(s => s.Trim());

            foreach (var searchTerm in searchTerms)
            {
                filteredQuery = filteredQuery
                    .Where(m => 
                        m.Title.ToLower().Contains(searchTerm) ||
                        m.Sinopse.ToLower().Contains(searchTerm));
            }
        }

        if (filter.Genres.Any())
            filteredQuery = filteredQuery.Where(m => m.FilmGenres.Any(g => filter.Genres.Contains(g.Genre)));
        
        var resultList = await filteredQuery
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new MovieInfoDto
            {
                Id  = m.Id,
                Title = m.Title,
                Sinopse = m.Sinopse,
                Duration = m.Duration,
                ReleaseDate = m.ReleaseDate,
                FilmGenres = m.FilmGenres.Select(genre => genre.Genre).ToList(),
                ClassInd = m.ClassInd
            })
            .Skip((query.CurrentPage - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        var totalCount = await filteredQuery.CountAsync();
        
        return new(resultList, query.CurrentPage, query.PageSize, totalCount);
    }

    public async Task<PaginationReturn<MovieSelectorItemDto>> GetSelectorMovies(PaginationQuery query, MovieSelectorFilterDto filter)
    {
        var filteredQuery = _dbContext.Movies
            .AsNoTracking()
            .Where(m => m.DeactivatedAt == null);

        if (filter.Title != null)
        {
            var titleTerms = filter.Title.ToLower().Split(", ").Select(s => s.Trim());

            foreach (var searchTerm in titleTerms)
            {
                filteredQuery = filteredQuery
                    .Where(m => 
                        m.Title.ToLower().Contains(searchTerm));
            }
        }

        var items = await filteredQuery
            .OrderByDescending(m => m.CreatedAt)
            .Select(m => new MovieSelectorItemDto
            {
                Id = m.Id,
                Title = m.Title,
                ReleaseDate = m.ReleaseDate
            })
            .Skip((query.CurrentPage - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        var totalCount = await filteredQuery
            .Select(m => m.Id)
            .CountAsync();

        return new(items, query.CurrentPage, query.PageSize, totalCount);
    }

    public async Task<bool> HasMovieWithId(Guid id) =>
        await _dbContext.Movies.AnyAsync(m => m.Id == id);

    async Task<Movie?> IMovieReadOnlyRepository.GetMovieById(Guid id) => 
        await _dbContext.Movies
            .AsNoTracking()
            .Include(m => m.FilmGenres)
            .FirstOrDefaultAsync(m => m.Id == id && m.DeactivatedAt == null);
    
    async Task<Movie?> IMovieUpdateOnlyRepository.GetMovieById(Guid id) => 
        await _dbContext.Movies
        .Include(m => m.FilmGenres)
        .FirstOrDefaultAsync(m => m.Id == id && m.DeactivatedAt == null);
}
