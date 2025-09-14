using FluentAssertions;
using Maybe;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains unit tests for the NotFoundError class.
/// </summary>
public class NotFoundErrorTests
{
    [Fact]
    public void ParameterlessConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var error = new NotFoundError();

        // Assert
        error.Type.Should().Be(OutcomeType.NotFound);
        error.Code.Should().Be("Default.NotFound");
        error.Message.Should().Be("A 'not found' error has occurred.");
        error.EntityName.Should().BeEmpty();
        error.Identifier.ToString().Should().BeEmpty();
    }

    [Fact]
    public void ParameterizedConstructor_ShouldSetAllPropertiesCorrectly()
    {
        // Arrange
        var innerError = new Error();

        // Act
        var error = new NotFoundError(
            "User",
            123,
            "The specified user could not be found.",
            "User.NotFoundById",
            innerError);

        // Assert
        error.Type.Should().Be(OutcomeType.NotFound);
        error.Code.Should().Be("User.NotFoundById");
        error.Message.Should().Be("The specified user could not be found.");
        error.EntityName.Should().Be("User");
        error.Identifier.Should().Be(123);
        error.InnerError.Should().BeSameAs(innerError);
    }

    [Fact]
    public void ParameterizedConstructor_WithNullMessageAndCode_ShouldUseDefaults()
    {
        // Act
        var error = new NotFoundError("Product", "SKU-XYZ");

        // Assert
        error.Code.Should().Be("NotFound.Product");
        error.Message.Should().Be("Product with identifier 'SKU-XYZ' was not found.");
    }

    [Fact]
    public void Setters_ShouldAllowPropertyModification()
    {
        // Arrange
        var error = new NotFoundError();

        // Act
        error.EntityName = "Order";
        error.Identifier = 456;

        // Assert
        error.EntityName.Should().Be("Order");
        error.Identifier.Should().Be(456);
    }
}
