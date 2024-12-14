namespace TinyUrl.DTOs;

public class UrlMapping
{
    public string LongUrl { get; set; }
    public string TinyUrl { get; set; }
    public int HitCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastHitAt { get; set; }
}