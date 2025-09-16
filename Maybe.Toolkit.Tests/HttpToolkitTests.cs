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

    [Fact]
    public async Task TryPutAsync_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act
        var result = await client!.TryPutAsync("http://example.com", null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task TryPutAsync_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act
        var result = await client.TryPutAsync((string?)null, null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
    }

    [Fact]
    public async Task TryPutAsync_WithUriObject_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act
        var result = await client.TryPutAsync((Uri?)null, null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task TryPatchAsync_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act
        var result = await client!.TryPatchAsync("http://example.com", null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task TryPatchAsync_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act
        var result = await client.TryPatchAsync((string?)null, null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
    }

    [Fact]
    public async Task TryPatchAsync_WithUriObject_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act
        var result = await client.TryPatchAsync((Uri?)null, null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task TryDeleteAsync_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act
        var result = await client!.TryDeleteAsync("http://example.com");

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task TryDeleteAsync_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act
        var result = await client.TryDeleteAsync((string?)null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
    }

    [Fact]
    public async Task TryDeleteAsync_WithUriObject_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act
        var result = await client.TryDeleteAsync((Uri?)null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task TryPostAsync_WithUriObject_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act
        var result = await client!.TryPostAsync(new Uri("http://example.com"), null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public async Task TryPostAsync_WithUriObject_WithNullUri_ReturnsHttpError()
    {
        // Arrange
        using var client = new HttpClient();

        // Act
        var result = await client.TryPostAsync((Uri?)null, null);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    #region JSON integration tests

    [Fact]
    public async Task TryGetJsonAsync_WithNullClient_ReturnsHttpError()
    {
        // Arrange
        HttpClient? client = null;

        // Act
        var result = await client!.TryGetJsonAsync<TestObject>("http://example.com");

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpJsonError>();
        error.IsHttpError.Should().BeTrue();
    }

    [Fact]
    public async Task TryPostJsonAsync_WithNullClient_ReturnsJsonError()
    {
        // Arrange
        HttpClient? client = null;
        var testObject = new TestObject { Name = "Test", Value = 42 };

        // Act
        var result = await client!.TryPostJsonAsync("http://example.com", testObject);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpJsonError>();
        error.IsHttpError.Should().BeTrue();
    }

    [Fact]
    public async Task TryPutJsonAsync_WithNullClient_ReturnsJsonError()
    {
        // Arrange
        HttpClient? client = null;
        var testObject = new TestObject { Name = "Test", Value = 42 };

        // Act
        var result = await client!.TryPutJsonAsync("http://example.com", testObject);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpJsonError>();
        error.IsHttpError.Should().BeTrue();
    }

    [Fact]
    public async Task TryPatchJsonAsync_WithNullClient_ReturnsJsonError()
    {
        // Arrange
        HttpClient? client = null;
        var testObject = new TestObject { Name = "Test", Value = 42 };

        // Act
        var result = await client!.TryPatchJsonAsync("http://example.com", testObject);

        // Assert
        result.Should().NotBeNull();
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<HttpJsonError>();
        error.IsHttpError.Should().BeTrue();
    }

    #endregion

    private class TestObject
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }
}