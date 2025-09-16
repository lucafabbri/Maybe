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

    [Fact]
    public void TryDeserialize_WithInvalidUtf8Json_ReturnsJsonError()
    {
        // Arrange
        var invalidJson = "{\"Name\":\"John\",\"Age\":}";
        var utf8Bytes = System.Text.Encoding.UTF8.GetBytes(invalidJson);

        // Act
        var result = JsonToolkit.TryDeserialize<Person>(utf8Bytes);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<JsonError>();
        error.Code.Should().Be("Json.SerializationError");
    }

    [Fact]
    public void TryDeserialize_WithNullJson_ReturnsJsonError()
    {
        // Act
        var result = JsonToolkit.TryDeserialize<Person>((string)null!);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<JsonError>();
    }

    [Fact]
    public void TrySerialize_WithCustomOptions_ReturnsSuccess()
    {
        // Arrange
        var person = new Person { Name = "John", Age = 30 };
        var options = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
        };

        // Act
        var result = JsonToolkit.TrySerialize(person, options);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var json = result.ValueOrThrow();
        json.Should().Contain("name"); // camelCase
        json.Should().Contain("age");  // camelCase
    }

    [Fact]
    public void TryDeserialize_WithCustomOptions_ReturnsSuccess()
    {
        // Arrange
        var json = "{\"name\":\"John\",\"age\":30}"; // camelCase
        var options = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
        };

        // Act
        var result = JsonToolkit.TryDeserialize<Person>(json, options);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var person = result.ValueOrThrow();
        person.Name.Should().Be("John");
        person.Age.Should().Be(30);
    }

    [Fact]
    public void TryDeserialize_WithUtf8Bytes_AndCustomOptions_ReturnsSuccess()
    {
        // Arrange
        var json = "{\"name\":\"John\",\"age\":30}"; // camelCase
        var utf8Bytes = System.Text.Encoding.UTF8.GetBytes(json);
        var options = new System.Text.Json.JsonSerializerOptions
        {
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
        };

        // Act
        var result = JsonToolkit.TryDeserialize<Person>(utf8Bytes, options);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var person = result.ValueOrThrow();
        person.Name.Should().Be("John");
        person.Age.Should().Be(30);
    }

    [Fact]
    public void TrySerialize_WithNullValue_ReturnsSuccess()
    {
        // Act
        var result = JsonToolkit.TrySerialize<Person?>(null);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var json = result.ValueOrThrow();
        json.Should().Be("null");
    }

    [Fact]
    public void TryDeserialize_ReturningNull_ReturnsJsonError()
    {
        // This tests the case where JsonSerializer.Deserialize returns null
        // but we expect a non-nullable type
        var json = "null";

        // Act
        var result = JsonToolkit.TryDeserialize<Person>(json);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<JsonError>();
        // The actual error message depends on the JsonError implementation
        error.Message.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void TryDeserialize_WithUtf8Bytes_ReturningNull_ReturnsJsonError()
    {
        // This tests the case where JsonSerializer.Deserialize returns null
        // but we expect a non-nullable type
        var json = "null";
        var utf8Bytes = System.Text.Encoding.UTF8.GetBytes(json);

        // Act
        var result = JsonToolkit.TryDeserialize<Person>(utf8Bytes);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<JsonError>();
        // The actual error message depends on the JsonError implementation
        error.Message.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void JsonError_Properties_AreSetCorrectly()
    {
        // Test the error class properties for better coverage
        var originalException = new System.Text.Json.JsonException("test json error");
        var error = new JsonError(originalException, "Test message");
        
        error.OriginalException.Should().Be(originalException);
        error.Message.Should().NotBeNullOrEmpty(); // The message behavior depends on JsonError implementation
        error.Code.Should().Be("Json.SerializationError");
    }

    [Fact]
    public void TrySerialize_WithCircularReference_ReturnsJsonError()
    {
        // Create an object with circular reference to test error handling
        var obj1 = new CircularObject { Name = "obj1" };
        var obj2 = new CircularObject { Name = "obj2", Reference = obj1 };
        obj1.Reference = obj2; // Create circular reference

        // Act
        var result = JsonToolkit.TrySerialize(obj1);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<JsonError>();
        error.Code.Should().Be("Json.SerializationError");
    }

    private class Person
    {
        public string Name { get; set; } = "";
        public int Age { get; set; }
    }

    private class CircularObject
    {
        public string Name { get; set; } = "";
        public CircularObject? Reference { get; set; }
    }
}