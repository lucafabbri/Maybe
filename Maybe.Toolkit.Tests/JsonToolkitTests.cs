using FluentAssertions;
using Maybe;
using Maybe.Toolkit;

namespace Maybe.Toolkit.Tests;

public class JsonToolkitTests
{
    [Fact]
    public void TryDeserialize_WithValidJson_ReturnsSuccess()
    {
        // Arrange
        var json = "{\"Name\":\"John\",\"Age\":30}";

        // Act
        var result = JsonToolkit.TryDeserialize<Person>(json);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var person = result.ValueOrThrow();
        person.Name.Should().Be("John");
        person.Age.Should().Be(30);
    }

    [Fact]
    public void TryDeserialize_WithInvalidJson_ReturnsJsonError()
    {
        // Arrange
        var invalidJson = "{\"Name\":\"John\",\"Age\":}"; // Missing value

        // Act
        var result = JsonToolkit.TryDeserialize<Person>(invalidJson);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<JsonError>();
        error.Code.Should().Be("Json.SerializationError");
    }

    [Fact]
    public void TryDeserialize_WithNullOrEmptyJson_ReturnsJsonError()
    {
        // Act & Assert
        JsonToolkit.TryDeserialize<Person>("").IsError.Should().BeTrue();
        JsonToolkit.TryDeserialize<Person>("   ").IsError.Should().BeTrue();
    }

    [Fact]
    public void TrySerialize_WithValidObject_ReturnsSuccess()
    {
        // Arrange
        var person = new Person { Name = "John", Age = 30 };

        // Act
        var result = JsonToolkit.TrySerialize(person);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var json = result.ValueOrThrow();
        json.Should().Contain("John");
        json.Should().Contain("30");
    }

    [Fact]
    public void TryDeserialize_WithUtf8Bytes_ReturnsSuccess()
    {
        // Arrange
        var json = "{\"Name\":\"John\",\"Age\":30}";
        var utf8Bytes = System.Text.Encoding.UTF8.GetBytes(json);

        // Act
        var result = JsonToolkit.TryDeserialize<Person>(utf8Bytes);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var person = result.ValueOrThrow();
        person.Name.Should().Be("John");
        person.Age.Should().Be(30);
    }

    [Fact]
    public void TryDeserialize_WithEmptyUtf8Bytes_ReturnsJsonError()
    {
        // Arrange
        var emptyBytes = new byte[0];

        // Act
        var result = JsonToolkit.TryDeserialize<Person>(emptyBytes);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<JsonError>();
    }

    private class Person
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }
}