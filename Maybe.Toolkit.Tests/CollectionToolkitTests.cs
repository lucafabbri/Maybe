using FluentAssertions;
using Maybe;
using Maybe.Toolkit;

namespace Maybe.Toolkit.Tests;

public class CollectionToolkitTests
{
    [Fact]
    public void TryGetValue_WithExistingKey_ReturnsSuccess()
    {
        // Arrange
        IDictionary<string, int> dictionary = new Dictionary<string, int>
        {
            { "key1", 100 },
            { "key2", 200 }
        };

        // Act
        var result = dictionary.TryGetValue("key1");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(100);
    }

    [Fact]
    public void TryGetValue_WithNonExistingKey_ReturnsCollectionError()
    {
        // Arrange
        IDictionary<string, int> dictionary = new Dictionary<string, int>
        {
            { "key1", 100 }
        };

        // Act
        var result = dictionary.TryGetValue("nonexistent");

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<CollectionError>();
        error.Code.Should().Be("Collection.AccessError");
        error.Key.Should().Be("nonexistent");
    }

    [Fact]
    public void TryGetValue_WithNullDictionary_ReturnsCollectionError()
    {
        // Arrange
        IDictionary<string, int>? dictionary = null;

        // Act
        var result = dictionary!.TryGetValue("key1");

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<CollectionError>();
    }

    [Fact]
    public void TryGetAt_WithValidIndex_ReturnsSuccess()
    {
        // Arrange
        IList<string> list = new List<string> { "first", "second", "third" };

        // Act
        var result = list.TryGetAt(1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be("second");
    }

    [Fact]
    public void TryGetAt_WithInvalidIndex_ReturnsCollectionError()
    {
        // Arrange
        IList<string> list = new List<string> { "first", "second" };

        // Act
        var result = list.TryGetAt(5);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<CollectionError>();
        error.Key.Should().Be(5);
    }

    [Fact]
    public void TryGetAt_WithNegativeIndex_ReturnsCollectionError()
    {
        // Arrange
        IList<string> list = new List<string> { "first", "second" };

        // Act
        var result = list.TryGetAt(-1);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<CollectionError>();
        error.Key.Should().Be(-1);
    }

    [Fact]
    public void TryGetAt_WithArray_ReturnsSuccess()
    {
        // Arrange
        var array = new[] { "first", "second", "third" };

        // Act
        var result = array.TryGetAt(2);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be("third");
    }

    [Fact]
    public void TryFirst_WithNonEmptySequence_ReturnsSuccess()
    {
        // Arrange
        var sequence = new[] { "first", "second", "third" };

        // Act
        var result = sequence.TryFirst();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be("first");
    }

    [Fact]
    public void TryFirst_WithEmptySequence_ReturnsCollectionError()
    {
        // Arrange
        var sequence = Array.Empty<string>();

        // Act
        var result = sequence.TryFirst();

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<CollectionError>();
        error.Key.Should().Be("first");
    }

    [Fact]
    public void TryLast_WithNonEmptySequence_ReturnsSuccess()
    {
        // Arrange
        var sequence = new[] { "first", "second", "third" };

        // Act
        var result = sequence.TryLast();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be("third");
    }

    [Fact]
    public void TryLast_WithEmptySequence_ReturnsCollectionError()
    {
        // Arrange
        var sequence = Array.Empty<string>();

        // Act
        var result = sequence.TryLast();

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<CollectionError>();
        error.Key.Should().Be("last");
    }

    [Fact]
    public void TryGetValue_WithReadOnlyDictionary_ReturnsSuccess()
    {
        // Arrange
        var dictionary = new Dictionary<string, int> { { "key1", 100 } };
        IReadOnlyDictionary<string, int> readOnlyDict = dictionary;

        // Act
        var result = readOnlyDict.TryGetValue("key1");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(100);
    }

    [Fact]
    public void TryGetAt_WithReadOnlyList_ReturnsSuccess()
    {
        // Arrange
        var list = new List<string> { "first", "second" };
        IReadOnlyList<string> readOnlyList = list;

        // Act
        var result = readOnlyList.TryGetAt(0);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be("first");
    }
}