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

    [Theory]
    [InlineData("1234567890", 1234567890L)]
    [InlineData("-987654321", -987654321L)]
    [InlineData("0", 0L)]
    public void TryParseLong_WithValidInput_ReturnsSuccess(string input, long expected)
    {
        // Act
        var result = ParseToolkit.TryParseLong(input);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(expected);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("12.34")]
    [InlineData("")]
    [InlineData("   ")]
    public void TryParseLong_WithInvalidInput_ReturnsParseError(string input)
    {
        // Act
        var result = ParseToolkit.TryParseLong(input);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<ParseError>();
        error.TargetType.Should().Be(typeof(long));
    }

    [Theory]
    [InlineData("123.45", 123.45)]
    [InlineData("-456.78", -456.78)]
    [InlineData("0.0", 0.0)]
    public void TryParseDecimal_WithValidInput_ReturnsSuccess(string input, decimal expected)
    {
        // Act
        var result = ParseToolkit.TryParseDecimal(input);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(expected);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("")]
    [InlineData("   ")]
    public void TryParseDecimal_WithInvalidInput_ReturnsParseError(string input)
    {
        // Act
        var result = ParseToolkit.TryParseDecimal(input);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<ParseError>();
        error.TargetType.Should().Be(typeof(decimal));
    }

    [Fact]
    public void TryParseInt_WithCustomNumberStyles_ReturnsSuccess()
    {
        // Act
        var result = ParseToolkit.TryParseInt("FF", System.Globalization.NumberStyles.HexNumber);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(255);
    }

    [Fact]
    public void TryParseInt_WithCustomFormatProvider_ReturnsSuccess()
    {
        // Arrange
        var culture = new System.Globalization.CultureInfo("en-US");

        // Act
        var result = ParseToolkit.TryParseInt("1,234", System.Globalization.NumberStyles.Number, culture);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(1234);
    }

    [Fact]
    public void TryParseLong_WithCustomNumberStyles_ReturnsSuccess()
    {
        // Act
        var result = ParseToolkit.TryParseLong("FF", System.Globalization.NumberStyles.HexNumber);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(255L);
    }

    [Fact]
    public void TryParseDouble_WithCustomNumberStyles_ReturnsSuccess()
    {
        // Arrange
        var culture = new System.Globalization.CultureInfo("en-US");

        // Act
        var result = ParseToolkit.TryParseDouble("1,234.56", System.Globalization.NumberStyles.Number, culture);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(1234.56);
    }

    [Fact]
    public void TryParseDecimal_WithCustomNumberStyles_ReturnsSuccess()
    {
        // Arrange
        var culture = new System.Globalization.CultureInfo("en-US");

        // Act
        var result = ParseToolkit.TryParseDecimal("1,234.56", System.Globalization.NumberStyles.Number, culture);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(1234.56m);
    }

    [Fact]
    public void TryParseDateTime_WithCustomFormatProvider_ReturnsSuccess()
    {
        // Arrange
        var culture = new System.Globalization.CultureInfo("en-US");
        var dateString = "12/25/2023 10:30:00 AM";

        // Act
        var result = ParseToolkit.TryParseDateTime(dateString, culture);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var parsed = result.ValueOrThrow();
        parsed.Year.Should().Be(2023);
        parsed.Month.Should().Be(12);
        parsed.Day.Should().Be(25);
    }

    [Fact]
    public void TryParseDateTime_WithCustomDateTimeStyles_ReturnsSuccess()
    {
        // Arrange
        var dateString = "2023-12-25T10:30:00Z";

        // Act
        var result = ParseToolkit.TryParseDateTime(dateString, null, System.Globalization.DateTimeStyles.AdjustToUniversal);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public void TryParseInt_WithNull_ReturnsParseError()
    {
        // Act
        var result = ParseToolkit.TryParseInt(null!);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<ParseError>();
    }

    [Fact]
    public void TryParseLong_WithNull_ReturnsParseError()
    {
        // Act
        var result = ParseToolkit.TryParseLong(null!);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<ParseError>();
    }

    [Fact]
    public void TryParseDouble_WithNull_ReturnsParseError()
    {
        // Act
        var result = ParseToolkit.TryParseDouble(null!);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<ParseError>();
    }

    [Fact]
    public void TryParseDecimal_WithNull_ReturnsParseError()
    {
        // Act
        var result = ParseToolkit.TryParseDecimal(null!);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<ParseError>();
    }

    [Fact]
    public void TryParseGuid_WithNull_ReturnsParseError()
    {
        // Act
        var result = ParseToolkit.TryParseGuid(null!);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<ParseError>();
    }

    [Fact]
    public void TryParseDateTime_WithNull_ReturnsParseError()
    {
        // Act
        var result = ParseToolkit.TryParseDateTime(null!);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<ParseError>();
    }

    [Fact]
    public void TryParseBool_WithNull_ReturnsParseError()
    {
        // Act
        var result = ParseToolkit.TryParseBool(null!);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<ParseError>();
    }

    [Fact]
    public void ParseError_Properties_AreSetCorrectly()
    {
        // Test the error class properties for better coverage
        var originalException = new FormatException("test format error");
        var error = new ParseError("test input", typeof(int), originalException, "Test message");
        
        error.OriginalException.Should().Be(originalException);
        error.InputValue.Should().Be("test input");
        error.TargetType.Should().Be(typeof(int));
        error.Message.Should().NotBeNullOrEmpty(); // The message behavior depends on ParseError implementation
        error.Code.Should().Be("Parse.FormatError");
    }
}