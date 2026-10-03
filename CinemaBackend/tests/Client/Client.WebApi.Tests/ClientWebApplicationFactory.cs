using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using TestsCommonUtilities;

namespace Client.WebApi.Tests;

public class ClientWebApplicationFactory : CommonWebApplicationFactory<Program>
{
    private readonly IConfiguration _configuration;
    public ClientWebApplicationFactory()
    {
        var config = new Dictionary<string, string?>
            {
                { "Jwt:SigningKey", "TEST-CLIEN--JWT-KEY-WITH-32-CHAR" },
                { "Jwt:Issuer", "CinemaBackendClient" },
                { "Jwt:Audience", "CinemaBackendClient" },
                { "Jwt:AccessTokenExpirationTimeMinutes", "1000" },
                { "Jwt:RefreshTokenExpirationTimeMinutes", "3600" }
            };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(config)
            .Build();
    }

    protected override IConfiguration GetConfiguration() => _configuration;
}
