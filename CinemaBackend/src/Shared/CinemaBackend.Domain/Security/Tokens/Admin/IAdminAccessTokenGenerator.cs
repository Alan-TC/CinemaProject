using CinemaBackend.Domain.Entities.AdminUserEntities;

namespace CinemaBackend.Domain.Security.Tokens.Admin;

public interface IAdminAccessTokenGenerator
{
    string Generate(AdminUser adminUser);
}
