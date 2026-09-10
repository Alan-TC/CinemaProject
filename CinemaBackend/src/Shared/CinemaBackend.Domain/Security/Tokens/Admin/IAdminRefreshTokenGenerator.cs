using CinemaBackend.Domain.Entities.AdminUserEntities;

namespace CinemaBackend.Domain.Security.Tokens.Admin;

public interface IAdminRefreshTokenGenerator
{
    string Generate(AdminUser adminUser);
}