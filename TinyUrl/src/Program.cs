using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using TinyUrl.Services;

namespace TinyUrl;

public class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("TinyUrl/src/appsettings.json")
            .Build();

        var serviceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddSingleton<IDisplayService, DisplayService>()
            .AddSingleton<IUrlMappingService, UrlMappingService>()
            .AddSingleton<IUrlGenerationService, UrlGenerationService>()
            .BuildServiceProvider();

        var displayService = serviceProvider.GetRequiredService<IDisplayService>();
        displayService.Start();
    }
}
