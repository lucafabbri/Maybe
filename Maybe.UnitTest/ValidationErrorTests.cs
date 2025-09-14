using FluentAssertions;
using Maybe;
using System.Collections.Generic;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains unit tests for the ValidationError class.
/// </summary>
public class ValidationErrorTests
{
    [Fact]
    public void ParameterlessConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var error = new ValidationError();

        // Assert
        error.Type.Should().Be(OutcomeType.Validation);
        error.Code.Should().Be("Default.Validation");
        error.Message.Should().Be("A validation error has occurred.");
        error.FieldErrors.Should().NotBeNull();
        error.FieldErrors.Should().BeEmpty();
    }

    [Fact]
    public void ParameterizedConstructor_ShouldSetAllPropertiesCorrectly()
    {
        // Arrange
        var innerError = new Error();
        var fieldErrors = new Dictionary<string, string>
        {
            { "Email", "Must be a valid email address." }
        };

        // Act
        var error = new ValidationError(
            fieldErrors,
            "User input is not valid.",
            "User.InvalidInput",
            innerError);

        // Assert
        error.Type.Should().Be(OutcomeType.Validation);
        error.Code.Should().Be("User.InvalidInput");
        error.Message.Should().Be("User input is not valid.");
        error.FieldErrors.Should().BeSameAs(fieldErrors);
        error.InnerError.Should().BeSameAs(innerError);
    }

    [Fact]
    public void Setters_ShouldAllowPropertyModification()
    {
        // Arrange
        var error = new ValidationError();
        var newFieldErrors = new Dictionary<string, string> { { "Password", "Is too short." } };

        // Act
        error.FieldErrors = newFieldErrors;

        // Assert
        error.FieldErrors.Should().BeSameAs(newFieldErrors);
    }
}
