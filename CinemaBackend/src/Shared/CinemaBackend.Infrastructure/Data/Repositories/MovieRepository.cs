using CinemaBackend.Domain.Entities;
using CinemaBackend.Domain.Repositories.Movie;

namespace CinemaBackend.Infrastructure.Data.Repositories;

internal class MovieRepository(CinemaDbContext dbContext) : IMovieWriteOnlyRepository
{
    private readonly CinemaDbContext _dbContext = dbContext;

    public async Task Add(Movie movie) => await _dbContext.Movies.AddAsync(movie);
}
