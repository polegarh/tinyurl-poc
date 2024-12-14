using TinyUrl.DTOs;
using TinyUrl.Exceptions;
using Microsoft.Extensions.Configuration;

namespace TinyUrl.Services;

public class UrlMappingService : IUrlMappingService
{
    private readonly Dictionary<string, UrlMapping> _urlMapping = new Dictionary<string, UrlMapping>();
    private readonly IUrlGenerationService _urlGenerationService;
    private readonly int _maxAttempts;
    private readonly string _hostName;

    public UrlMappingService(IUrlGenerationService urlGenerationService, IConfiguration configuration)
    {
        _urlMapping = new Dictionary<string, UrlMapping>();
        _urlGenerationService = urlGenerationService;
        _maxAttempts = configuration["UrlSettings:MaxGenerationAttempts"] != null 
            ? int.Parse(configuration["UrlSettings:MaxGenerationAttempts"]) 
            : throw new UrlMappingException("Configuration Error: Max generation attempts are not set");        
        _hostName = configuration["UrlSettings:HostName"] 
            ?? throw new UrlMappingException("Configuration Error: Host name is not set");
    }

    public string CreateUrl(string longUrl, string? customTinyUrl = null)
    {
        string? tinyUrl;
        if (customTinyUrl != null){
            if (_urlMapping.ContainsKey(customTinyUrl)){
                throw new UrlMappingException("Tiny URL already exists");
            }
            tinyUrl = _hostName + customTinyUrl;
        } else {
            var generatedTinyString = GenerateUnqueTinyUrl();
            tinyUrl = _hostName + generatedTinyString;
        }

        _urlMapping[tinyUrl] = new UrlMapping { LongUrl = longUrl, TinyUrl = tinyUrl, HitCount = 0, CreatedAt = DateTime.Now, LastHitAt = null };
        return tinyUrl;
    }

    public string GetLongUrl(string tinyUrl)
    {
        if (!_urlMapping.ContainsKey(tinyUrl))
        {
            throw new UrlMappingException("Tiny URL not found");
        }
        _urlMapping[tinyUrl].HitCount++;
        _urlMapping[tinyUrl].LastHitAt = DateTime.Now;
        return _urlMapping[tinyUrl].LongUrl;
    }

    public void DeleteUrl(string tinyUrl)
    {
        if (!_urlMapping.ContainsKey(tinyUrl))
        {
            throw new UrlMappingException("Tiny URL not found");
        }
        _urlMapping.Remove(tinyUrl);
    }

    public int GetHitCount(string tinyUrl)
    {
        if (!_urlMapping.ContainsKey(tinyUrl))
        {
            throw new UrlMappingException("Tiny URL not found");
        }
        return _urlMapping[tinyUrl].HitCount;
    }

    private string GenerateUnqueTinyUrl()
    {
        int attempts = 0;
        while (attempts < _maxAttempts)
        {
            var tinyUrl = _urlGenerationService.GenerateTinyUrl();
            
            if (!_urlMapping.ContainsKey(tinyUrl))
            {
                return tinyUrl;
            }
            
            attempts++;
        }
        throw new UrlMappingException("Failed to generate a unique tiny URL after maximum attempts");
    }
}
