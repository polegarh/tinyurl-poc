using Xunit;
using TinyUrl.Services;
using TinyUrl.Tests.Helpers;
using TinyUrl.Exceptions;

namespace TinyUrl.Tests.Services;

public class UrlGenerationServiceTests
{
    private readonly UrlGenerationService _service;

    public UrlGenerationServiceTests()
    {
        var configuration = TestConfiguration.GetConfiguration();
        _service = new UrlGenerationService(configuration);
    }

    [Fact]
    public void GenerateTinyUrl_ShouldReturnStringOfCorrectLength()
    {
        // Act
        var result = _service.GenerateTinyUrl();

        // Assert
        Assert.Equal(6, result.Length);
    }

    [Fact]
    public void GenerateTinyUrl_ShouldOnlyContainAllowedCharacters()
    {
        // Arrange
        var allowedChars = TestConfiguration.GetConfiguration()["UrlSettings:AllowedChars"];

        // Act
        var result = _service.GenerateTinyUrl();

        // Assert
        Assert.All(result, c => Assert.Contains(c, allowedChars));
    }

    [Fact]
    public void GenerateTinyUrl_ShouldGenerateRandomStrings()
    {
        // Act
        var result1 = _service.GenerateTinyUrl();
        var result2 = _service.GenerateTinyUrl();

        // Assert
        Assert.NotEqual(result1, result2);
    }
} 