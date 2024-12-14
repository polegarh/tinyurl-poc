using Xunit;
using Moq;
using TinyUrl.Services;
using TinyUrl.Exceptions;
using TinyUrl.Tests.Helpers;

namespace TinyUrl.Tests.Services;

public class UrlMappingServiceTests
{
    private readonly UrlMappingService _service;
    private readonly Mock<IUrlGenerationService> _urlGenerationServiceMock;
    private const string TestHostName = "http://test-tiny-url.com/";

    public UrlMappingServiceTests()
    {
        _urlGenerationServiceMock = new Mock<IUrlGenerationService>();
        var configuration = TestConfiguration.GetConfiguration();
        _service = new UrlMappingService(_urlGenerationServiceMock.Object, configuration);
    }

    [Theory]
    [InlineData("http://example.com", "abc123")]
    [InlineData("http://facebook.com", "ajkd11")]
    public void CreateUrl_NoCustom_ShouldCreateMapping(string givenUrl, string generatedTinyUrl)
    {
        // Arrange
        _urlGenerationServiceMock.Setup(x => x.GenerateTinyUrl())
            .Returns(generatedTinyUrl);

        // Act
        var result = _service.CreateUrl(givenUrl);

        // Assert
        Assert.Equal($"{TestHostName}{generatedTinyUrl}", result);
    }

    [Theory]
    [InlineData("http://example.com", "custo1")]
    [InlineData("http://google.com", "custo2")]
    public void CreateUrl_WithCustom_ShouldUseCustomUrl(string givenUrl, string customUrl)
    {
        // Act
        var result = _service.CreateUrl(givenUrl, customUrl);

        // Assert
        Assert.Equal($"{TestHostName}{customUrl}", result);
    }

    [Fact]
    public void GetLongUrl_WithValidTinyUrl_ShouldReturnLongUrl()
    {
        // Arrange
        var longUrl = "http://example.com";
        var tinyUrl = $"{TestHostName}abc123";
        _service.CreateUrl(longUrl, "abc123");

        // Act
        var result = _service.GetLongUrl(tinyUrl);

        // Assert
        Assert.Equal(longUrl, result);
    }

    [Fact]
    public void GetLongUrl_WithInvalidTinyUrl_ShouldThrowException()
    {
        // Arrange
        var invalidTinyUrl = $"{TestHostName}nonexistent";

        // Act & Assert
        Assert.Throws<UrlMappingException>(() => _service.GetLongUrl(invalidTinyUrl));
    }

    [Fact]
    public void GetHitCount_WithValidTinyUrl_ShouldReturnHits()
    {
        // Arrange
        var longUrl = "http://example.com";
        var tinyUrl = $"{TestHostName}abc123";
        _service.CreateUrl(longUrl, "abc123");

        // Act
        var result = _service.GetHitCount(tinyUrl);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetHitCount_WithInvalidTinyUrl_ShouldThrowException()
    {
        // Arrange
        var invalidTinyUrl = $"{TestHostName}nonexistent";

        // Act & Assert
        Assert.Throws<UrlMappingException>(() => _service.GetHitCount(invalidTinyUrl));
    }
} 