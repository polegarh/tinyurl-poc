using Microsoft.Extensions.Configuration;

namespace TinyUrl.Tests.Helpers;

public static class TestConfiguration
{
    public static IConfiguration GetConfiguration()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"UrlSettings:AllowedChars", "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"},
                {"UrlSettings:TinyUrlLength", "6"},
                {"UrlSettings:MaxGenerationAttempts", "3"},
                {"UrlSettings:HostName", "http://test-tiny-url.com/"}
            })
            .Build();

        return config;
    }
} 