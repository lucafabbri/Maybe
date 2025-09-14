using FluentAssertions;
using Maybe;
using System.Collections.Generic;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains unit tests for the ConflictError class.
/// </summary>
public class ConflictErrorTests
{
    [Fact]
    public void ParameterlessConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var error = new ConflictError();

        // Assert
        error.Type.Should().Be(OutcomeType.Conflict);
        error.Code.Should().Be("Default.Conflict");
        error.Message.Should().Be("A conflict error has occurred.");
        error.ConflictType.Should().Be(ConflictType.BusinessRuleViolation);
        error.ResourceType.Should().BeEmpty();
        error.ConflictingParameters.Should().BeEmpty();
    }

    [Fact]
    public void ParameterizedConstructor_ShouldSetAllPropertiesCorrectly()
    {
        // Arrange
        var innerError = new Error();
        var parameters = new Dictionary<string, object> { { "Email", "test@test.com" } };

        // Act
        var error = new ConflictError(
            ConflictType.Duplicate,
            "User",
            parameters,
            "User already exists.",
            "Auth.DuplicateUser",
            innerError);

        // Assert
        error.Type.Should().Be(OutcomeType.Conflict);
        error.Code.Should().Be("Auth.DuplicateUser");
        error.Message.Should().Be("User already exists.");
        error.ConflictType.Should().Be(ConflictType.Duplicate);
        error.ResourceType.Should().Be("User");
        error.ConflictingParameters.Should().BeSameAs(parameters);
        error.InnerError.Should().BeSameAs(innerError);
    }

    [Fact]
    public void ParameterizedConstructor_WithNullMessageAndCode_ShouldUseDefaults()
    {
        // Arrange
        var parameters = new Dictionary<string, object>();

        // Act
        var error = new ConflictError(
            ConflictType.StaleState,
            "Product",
            parameters);

        // Assert
        error.Code.Should().Be("Conflict.StaleState");
        error.Message.Should().Be("A StaleState conflict occurred on resource 'Product'.");
    }

    [Fact]
    public void Setters_ShouldAllowPropertyModification()
    {
        // Arrange
        var error = new ConflictError();
        var newParameters = new Dictionary<string, object> { { "Version", 2 } };

        // Act
        error.ConflictType = ConflictType.StaleState;
        error.ResourceType = "Order";
        error.ConflictingParameters = newParameters;

        // Assert
        error.ConflictType.Should().Be(ConflictType.StaleState);
        error.ResourceType.Should().Be("Order");
        error.ConflictingParameters.Should().BeSameAs(newParameters);
    }
}
