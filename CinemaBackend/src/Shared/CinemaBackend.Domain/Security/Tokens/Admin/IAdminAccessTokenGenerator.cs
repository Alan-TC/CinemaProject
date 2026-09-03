using CinemaBackend.Domain.Entities;

namespace CinemaBackend.Domain.Security.Tokens.Admin;

public interface IAdminAccessTokenGenerator
{
    string Generate(AdminUser adminUser);
}
