using FluentAssertions;
using Maybe;
using System.Collections.Generic;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains unit tests for the FailureError class.
/// </summary>
public class FailureErrorTests
{
    [Fact]
    public void ParameterlessConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var error = new FailureError();

        // Assert
        error.Type.Should().Be(OutcomeType.Failure);
        error.Code.Should().Be("Default.Failure");
        error.Message.Should().Be("A failure has occurred.");
        error.ContextData.Should().BeEmpty();
    }

    [Fact]
    public void ParameterizedConstructor_ShouldSetAllPropertiesCorrectly()
    {
        // Arrange
        var innerError = new Error();
        var context = new Dictionary<string, object> { { "TransactionId", "123" } };

        // Act
        var error = new FailureError(
            "Payment failed",
            "Payment.GatewayError",
            context,
            innerError);

        // Assert
        error.Type.Should().Be(OutcomeType.Failure);
        error.Code.Should().Be("Payment.GatewayError");
        error.Message.Should().Be("Payment failed");
        error.ContextData.Should().BeSameAs(context);
        error.InnerError.Should().BeSameAs(innerError);
    }

    [Fact]
    public void ParameterizedConstructor_WithNullContextData_ShouldInitializeEmptyDictionary()
    {
        // Act
        var error = new FailureError(
            "Payment failed",
            "Payment.GatewayError",
            null, // Explicitly pass null
            null);

        // Assert
        error.ContextData.Should().NotBeNull();
        error.ContextData.Should().BeEmpty();
    }

    [Fact]
    public void Setters_ShouldAllowPropertyModification()
    {
        // Arrange
        var error = new FailureError();
        var newContext = new Dictionary<string, object> { { "Reason", "Timeout" } };

        // Act
        error.ContextData = newContext;

        // Assert
        error.ContextData.Should().BeSameAs(newContext);
    }
}
