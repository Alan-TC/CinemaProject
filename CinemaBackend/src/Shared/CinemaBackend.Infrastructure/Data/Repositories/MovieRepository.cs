using CinemaBackend.Domain.Entities;
using CinemaBackend.Domain.Repositories.Movie;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data.Repositories;

internal class MovieRepository(CinemaDbContext dbContext) : IMovieWriteOnlyRepository, IMovieReadOnlyRepository, IMovieUpdateOnlyRepository
{
    private readonly CinemaDbContext _dbContext = dbContext;

    public async Task Add(Movie movie) => await _dbContext.Movies.AddAsync(movie);

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
