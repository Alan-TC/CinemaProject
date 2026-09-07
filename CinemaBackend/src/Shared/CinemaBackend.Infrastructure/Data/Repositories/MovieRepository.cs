using CinemaBackend.Domain.Entities;
using CinemaBackend.Domain.Repositories.Movie;
using Microsoft.EntityFrameworkCore;

namespace CinemaBackend.Infrastructure.Data.Repositories;

internal class MovieRepository(CinemaDbContext dbContext) : IMovieWriteOnlyRepository, IMovieReadOnlyRepository
{
    private readonly CinemaDbContext _dbContext = dbContext;

    public async Task Add(Movie movie) => await _dbContext.Movies.AddAsync(movie);

    public async Task<Movie?> GetMovieById(Guid id) => 
        await _dbContext.Movies.FirstOrDefaultAsync(m => m.Id == id && m.DeactivatedAt == null);
}
