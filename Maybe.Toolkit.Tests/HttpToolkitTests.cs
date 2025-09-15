using FluentAssertions;
using Maybe;
using Maybe.Toolkit;
using System.Net;

namespace Maybe.Toolkit.Tests;

public class HttpToolkitTests
{
    [Fact]
    public void TryGetAsync_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act & Assert
        var result = client!.TryGetAsync("http://example.com");
        result.Should().NotBeNull();
        result.Result.IsError.Should().BeTrue();
        var error = result.Result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void TryGetAsync_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act & Assert
        var result = client.TryGetAsync((string?)null);
        result.Should().NotBeNull();
        result.Result.IsError.Should().BeTrue();
        var error = result.Result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
    }

    [Fact]
    public void TryGetAsync_WithEmptyUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act & Assert
        var result = client.TryGetAsync("");
        result.Should().NotBeNull();
        result.Result.IsError.Should().BeTrue();
        var error = result.Result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
    }

    [Fact]
    public void TryGetAsync_WithUriObject_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act & Assert
        var result = client.TryGetAsync((Uri?)null);
        result.Should().NotBeNull();
        result.Result.IsError.Should().BeTrue();
        var error = result.Result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void TryPostAsync_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act & Assert
        var result = client!.TryPostAsync("http://example.com", null);
        result.Should().NotBeNull();
        result.Result.IsError.Should().BeTrue();
        var error = result.Result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void TryGetStringAsync_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act & Assert
        var result = client!.TryGetStringAsync("http://example.com");
        result.Should().NotBeNull();
        result.Result.IsError.Should().BeTrue();
        var error = result.Result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
    }

    [Fact]
    public void TryGetByteArrayAsync_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act & Assert
        var result = client!.TryGetByteArrayAsync("http://example.com");
        result.Should().NotBeNull();
        result.Result.IsError.Should().BeTrue();
        var error = result.Result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
    }

    // Note: We're avoiding actual HTTP requests in unit tests to prevent:
    // 1. External dependencies
    // 2. Network-related test failures
    // 3. Slow test execution
    // In a real-world scenario, you would use mocking frameworks like Moq
    // or create integration tests that use test servers.
}