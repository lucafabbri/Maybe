using FluentAssertions;
using Maybe;
using Maybe.Toolkit;

namespace Maybe.Toolkit.Tests;

public class ParseToolkitTests
{
    [Theory]
    [InlineData("123", 123)]
    [InlineData("-456", -456)]
    [InlineData("0", 0)]
    public void TryParseInt_WithValidInput_ReturnsSuccess(string input, int expected)
    {
        // Act
        var result = ParseToolkit.TryParseInt(input);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(expected);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("12.34")]
    [InlineData("")]
    [InlineData("   ")]
    public void TryParseInt_WithInvalidInput_ReturnsParseError(string input)
    {
        // Act
        var result = ParseToolkit.TryParseInt(input);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<ParseError>();
        error.Code.Should().Be("Parse.FormatError");
        error.TargetType.Should().Be(typeof(int));
        error.InputValue.Should().Be(input);
    }

    [Theory]
    [InlineData("123.45", 123.45)]
    [InlineData("-456.78", -456.78)]
    [InlineData("0.0", 0.0)]
    public void TryParseDouble_WithValidInput_ReturnsSuccess(string input, double expected)
    {
        // Act
        var result = ParseToolkit.TryParseDouble(input);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(expected);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("")]
    [InlineData("   ")]
    public void TryParseDouble_WithInvalidInput_ReturnsParseError(string input)
    {
        // Act
        var result = ParseToolkit.TryParseDouble(input);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<ParseError>();
        error.TargetType.Should().Be(typeof(double));
    }

    [Fact]
    public void TryParseGuid_WithValidGuid_ReturnsSuccess()
    {
        // Arrange
        var expectedGuid = Guid.NewGuid();
        var guidString = expectedGuid.ToString();

        // Act
        var result = ParseToolkit.TryParseGuid(guidString);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(expectedGuid);
    }

    [Theory]
    [InlineData("invalid-guid")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("12345678")]
    public void TryParseGuid_WithInvalidInput_ReturnsParseError(string input)
    {
        // Act
        var result = ParseToolkit.TryParseGuid(input);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<ParseError>();
        error.TargetType.Should().Be(typeof(Guid));
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("false", false)]
    [InlineData("True", true)]
    [InlineData("False", false)]
    public void TryParseBool_WithValidInput_ReturnsSuccess(string input, bool expected)
    {
        // Act
        var result = ParseToolkit.TryParseBool(input);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(expected);
    }

    [Theory]
    [InlineData("yes")]
    [InlineData("no")]
    [InlineData("1")]
    [InlineData("0")]
    [InlineData("")]
    [InlineData("   ")]
    public void TryParseBool_WithInvalidInput_ReturnsParseError(string input)
    {
        // Act
        var result = ParseToolkit.TryParseBool(input);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<ParseError>();
        error.TargetType.Should().Be(typeof(bool));
    }

    [Fact]
    public void TryParseDateTime_WithValidInput_ReturnsSuccess()
    {
        // Arrange
        var dateString = "2023-12-25T10:30:00";
        var expected = new DateTime(2023, 12, 25, 10, 30, 0);

        // Act
        var result = ParseToolkit.TryParseDateTime(dateString);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(expected);
    }

    [Theory]
    [InlineData("invalid-date")]
    [InlineData("")]
    [InlineData("   ")]
    public void TryParseDateTime_WithInvalidInput_ReturnsParseError(string input)
    {
        // Act
        var result = ParseToolkit.TryParseDateTime(input);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<ParseError>();
        error.TargetType.Should().Be(typeof(DateTime));
    }
}