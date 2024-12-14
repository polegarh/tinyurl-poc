using Microsoft.Extensions.Configuration;
using TinyUrl.Exceptions;

namespace TinyUrl.Services;

public class UrlGenerationService : IUrlGenerationService
{
    private readonly string _allowedChars;
    private readonly int _tinyUrlLength;
    private readonly Random _random;

    public UrlGenerationService(IConfiguration configuration)
    {
        _random = new Random();
        _allowedChars = configuration["UrlSettings:AllowedChars"] 
            ?? throw new UrlGenerationException("Configuration Error: Allowed characters are not set");
        _tinyUrlLength = configuration["UrlSettings:TinyUrlLength"] != null 
            ? int.Parse(configuration["UrlSettings:TinyUrlLength"]) 
            : throw new UrlGenerationException("Configuration Error: Tiny URL length is not set");
    }

    public string GenerateTinyUrl()
    {
        var chars = new char[_tinyUrlLength];
        for (int i = 0; i < _tinyUrlLength; i++)
        {
            chars[i] = _allowedChars[_random.Next(0, _allowedChars.Length)];
        }
        return new string(chars);
    }
}