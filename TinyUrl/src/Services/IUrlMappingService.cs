namespace TinyUrl.Services;

public interface IUrlMappingService
{
    public string CreateUrl(string longUrl, string? customTinyUrl = null);
    public string GetLongUrl(string tinyUrl);
    public void DeleteUrl(string tinyUrl);
    public int GetHitCount(string tinyUrl);
}