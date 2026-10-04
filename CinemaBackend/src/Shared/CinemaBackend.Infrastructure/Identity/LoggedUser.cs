using CinemaBackend.Domain.Entities.UserEntities;
using CinemaBackend.Domain.Identity.Client;
using CinemaBackend.Domain.Security.Tokens.User;
using CinemaBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace CinemaBackend.Infrastructure.Identity;

internal class LoggedUser(
    IUserAccessTokenProvider accessTokenProvider,
    CinemaDbContext dbContext) : ILoggedUser
{
    private readonly IUserAccessTokenProvider _accessTokenProvider = accessTokenProvider;
    private readonly CinemaDbContext _dbContext = dbContext;

    public async Task<User> Get()
    {
        var userId = GetUserId();

        var user = await _dbContext.Users.AsNoTracking().FirstAsync(user => user.DeactivatedAt == null && user.Id == userId);

        return user;
    }

    public Guid GetUserId()
    {
        var accessToken = _accessTokenProvider.GetToken();

        var handler = new JsonWebTokenHandler();

        var jsonWebToken = handler.ReadJsonWebToken(accessToken);

        var subject = jsonWebToken.Subject;

        return Guid.Parse(subject);
    }
}
