namespace TinyUrl.Exceptions;

public class UrlMappingException : Exception
{
    public UrlMappingException(string message) : base(message)
    {
    }
}