using FluentAssertions;
using Maybe;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains unit tests for the abstract BaseError class.
/// </summary>
public class BaseErrorTests
{
    // A concrete implementation of BaseError for testing purposes.
    private class TestError : BaseError
    {
        public TestError(OutcomeType type, string code, string message)
        {
            Type = type;
            Code = code;
            Message = message;
        }
        public override OutcomeType Type { get; protected set; }
        public override string Code { get; protected set; }
        public override string Message { get; protected set; }
    }

    [Fact]
    public void ToString_ShouldReturnCorrectlyFormattedString()
    {
        // Arrange
        var error = new TestError(OutcomeType.Failure, "Test.Code", "This is a test message.");

        // Act
        var result = error.ToString();

        // Assert
        result.Should().Be("[Failure] Test.Code: This is a test message.");
    }

    [Fact]
    public void Equals_WithSameInstance_ShouldReturnTrue()
    {
        // Arrange
        var error = new TestError(OutcomeType.Failure, "Test.Code", "Message");

        // Act & Assert
        error.Equals(error).Should().BeTrue();
        error.Equals((object)error).Should().BeTrue();
    }

    [Fact]
    public void Equals_WithEquivalentInstance_ShouldReturnTrue()
    {
        // Arrange
        var error1 = new TestError(OutcomeType.Failure, "Test.Code", "Message 1");
        var error2 = new TestError(OutcomeType.Failure, "Test.Code", "Message 2"); // Same Type and Code matter

        // Act & Assert
        error1.Equals(error2).Should().BeTrue();
        error1.Equals((object)error2).Should().BeTrue();
    }

    [Theory]
    [InlineData(OutcomeType.Conflict, "Test.Code")] // Different Type
    [InlineData(OutcomeType.Failure, "Different.Code")] // Different Code
    public void Equals_WithDifferentInstance_ShouldReturnFalse(OutcomeType type, string code)
    {
        // Arrange
        var error1 = new TestError(OutcomeType.Failure, "Test.Code", "Message");
        var error2 = new TestError(type, code, "Message");

        // Act & Assert
        error1.Equals(error2).Should().BeFalse();
        error1.Equals((object)error2).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithNull_ShouldReturnFalse()
    {
        // Arrange
        var error = new TestError(OutcomeType.Failure, "Test.Code", "Message");

        // Act & Assert
        error.Equals(null).Should().BeFalse();
        error.Equals((object?)null).Should().BeFalse();
    }

    [Fact]
    public void Equals_WithDifferentObjectType_ShouldReturnFalse()
    {
        // Arrange
        var error = new TestError(OutcomeType.Failure, "Test.Code", "Message");
        var otherObject = new object();

        // Act & Assert
        error.Equals(otherObject).Should().BeFalse();
    }

    [Fact]
    public void SetInnerError_ShouldCorrectlyAssignInnerErrorProperty()
    {
        // Arrange
        var outerError = new TestError(OutcomeType.Failure, "Outer", "Outer message");
        var innerError = new TestError(OutcomeType.NotFound, "Inner", "Inner message");

        // Act
        outerError.SetInnerError(innerError);

        // Assert
        outerError.InnerError.Should().BeSameAs(innerError);
    }

    [Fact]
    public async Task Constructor_ShouldSetTimestampWithinRange()
    {
        // Arrange
        var before = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // Act
        await Task.Delay(1); // Ensure time progresses slightly
        var error = new TestError(OutcomeType.Failure, "Code", "Message");
        await Task.Delay(1);

        var after = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        // Assert
        error.TimeStamp.Should().BeInRange(before, after);
    }

    [Fact]
    public void ToFullString_WithSingleError_ShouldFormatCorrectlyWithoutIndentation()
    {
        // Arrange
        var error = new TestError(OutcomeType.Failure, "Test.Code", "A simple message.");
        var timestamp = $"[{DateTimeOffset.FromUnixTimeSeconds(error.TimeStamp).ToLocalTime():yyyy-MM-dd HH:mm:ss}]";

        // Act
        var result = error.ToFullString();

        // Assert
        result.Should().Contain("[Failure]");
        result.Should().Contain("Test.Code");
        result.Should().Contain(timestamp);
        result.Should().Contain("A simple message.");
        result.Should().NotStartWith(" ["); // No indentation for inner errors
    }

    [Fact]
    public void ToFullString_WithChainedErrors_ShouldFormatWithCorrectIndentation()
    {
        // Arrange
        var innerMostError = new TestError(OutcomeType.NotFound, "DB.Error", "Entity not found.");
        var middleError = new TestError(OutcomeType.Failure, "Service.Error", "Service failed.");
        var outerError = new TestError(OutcomeType.Unexpected, "API.Error", "API request failed.");

        (middleError as BaseError).SetInnerError(innerMostError);
        (outerError as BaseError).SetInnerError(middleError);

        // Act
        var result = outerError.ToFullString();
        var lines = result.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        // Assert
        lines.Should().HaveCount(3);
        lines[0].Should().NotStartWith(" ");
        lines[0].Should().Contain("[Unexpected]");
        lines[1].Should().StartWith("  ");
        lines[1].Should().Contain("[Failure]");
        lines[2].Should().StartWith("    ");
        lines[2].Should().Contain("[NotFound]");
    }

    [Fact]
    public void ToFullString_WithLongMessage_ShouldWrapTextCorrectly()
    {
        // Arrange
        var longMessage = "This is a very long message designed to exceed the maximum width of the output string to properly test the text wrapping functionality of the ToFullString method, ensuring it breaks into multiple lines.";
        var error = new TestError(OutcomeType.Validation, "Validation.Long", longMessage);

        // Act
        var result = error.ToFullString();
        var lines = result.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        // Assert
        lines.Length.Should().BeGreaterThan(1);
        lines[1].Should().NotBeEmpty();
        lines[1].Trim().Should().NotStartWith("["); // The second line is part of the message, not a new error.
    }

    [Fact]
    public void ToFullString_WithEmptyMessage_ShouldNotThrowAndFormatCorrectly()
    {
        // Arrange
        var error = new TestError(OutcomeType.Failure, "Test.Code", "");

        // Act
        var result = error.ToFullString();
        var lines = result.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        // Assert
        result.Should().NotBeNullOrEmpty();
        lines.Should().HaveCount(1);
        // The timestamp part should be followed by empty space for the message
        lines[0].Should().Contain("]");
    }
}
