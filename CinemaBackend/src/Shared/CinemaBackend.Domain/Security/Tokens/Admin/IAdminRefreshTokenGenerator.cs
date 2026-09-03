using CinemaBackend.Domain.Entities;

namespace CinemaBackend.Domain.Security.Tokens.Admin;

public interface IAdminRefreshTokenGenerator
{
    string Generate(AdminUser adminUser);
}