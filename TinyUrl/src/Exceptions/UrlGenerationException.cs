namespace TinyUrl.Exceptions;

public class UrlGenerationException : Exception
{
    public UrlGenerationException(string message) : base(message)
    {
    }
}