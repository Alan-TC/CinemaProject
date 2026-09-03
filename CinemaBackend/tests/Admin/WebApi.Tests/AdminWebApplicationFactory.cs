using Microsoft.Extensions.Configuration;
using TestsCommonUtilities;

namespace Admin.WebApi.Tests;

public class AdminWebApplicationFactory : CommonWebApplicationFactory<Program>
{
    private readonly IConfiguration _configuration;
    public AdminWebApplicationFactory()
    {
        var config = new Dictionary<string, string?> 
            {
                { "Jwt:SigningKey", "TEST-ADMIN--JWT-KEY-WITH-32-CHAR" },
                { "Jwt:Issuer", "CinemaBackendAdmin" },
                { "Jwt:Audience", "CinemaBackendAdmin" },
                { "Jwt:AccessTokenExpirationTimeMinutes", "1000" },
                { "Jwt:RefreshTokenExpirationTimeMinutes", "3600" }
            };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();
    }

    protected override IConfiguration GetConfiguration() => _configuration;
}
