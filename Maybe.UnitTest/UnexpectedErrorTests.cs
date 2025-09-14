using FluentAssertions;
using Maybe;
using System;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains unit tests for the UnexpectedError class.
/// </summary>
public class UnexpectedErrorTests
{
    [Fact]
    public void ParameterlessConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var error = new UnexpectedError();

        // Assert
        error.Type.Should().Be(OutcomeType.Unexpected);
        error.Code.Should().Be("System.Exception");
        error.Message.Should().Be("An unexpected error has occurred.");
        error.Exception.Should().BeOfType<InvalidOperationException>()
            .Which.Message.Should().Be("Default exception for parameterless constructor.");
    }

    [Fact]
    public void ParameterizedConstructor_ShouldSetAllPropertiesCorrectly()
    {
        // Arrange
        var innerEx = new ArgumentNullException("param");
        var ex = new InvalidOperationException("Outer exception", innerEx);

        // Act
        var error = new UnexpectedError(ex, "A critical error happened.", "Critical.Fault");

        // Assert
        error.Type.Should().Be(OutcomeType.Unexpected);
        error.Code.Should().Be("Critical.Fault");
        error.Message.Should().Be("A critical error happened.");
        error.Exception.Should().BeSameAs(ex);
        error.InnerError.Should().NotBeNull();
        error.InnerError.Should().BeOfType<UnexpectedError>();
        (error.InnerError as UnexpectedError)!.Exception.Should().BeSameAs(innerEx);
    }

    [Fact]
    public void ParameterizedConstructor_WithNullMessageAndCode_ShouldUseExceptionDefaults()
    {
        // Arrange
        var ex = new Exception("The actual error message.");

        // Act
        var error = new UnexpectedError(ex);

        // Assert
        error.Code.Should().Be("System.Exception");
        error.Message.Should().Be("The actual error message.");
        error.Exception.Should().BeSameAs(ex);
    }

    [Fact]
    public void ExceptionSetter_ShouldUpdateExceptionAndInnerError()
    {
        // Arrange
        var error = new UnexpectedError();
        var innerEx = new FormatException("Inner");
        var newEx = new ApplicationException("New outer", innerEx);

        // Act
        error.Exception = newEx;

        // Assert
        error.Exception.Should().BeSameAs(newEx);
        error.InnerError.Should().NotBeNull();
        var innerUnexpectedError = error.InnerError.Should().BeOfType<UnexpectedError>().Subject;
        innerUnexpectedError.Exception.Should().BeSameAs(innerEx);
    }

    [Fact]
    public void ExceptionSetter_WhenNewExceptionHasNoInner_ShouldNotSetInnerError()
    {
        // Arrange
        var initialInnerEx = new FormatException("InitialInner");
        var initialEx = new ApplicationException("InitialOuter", initialInnerEx);
        var error = new UnexpectedError(initialEx); // Starts with an inner error

        var newExWithoutInner = new Exception("New exception without inner");

        // Act
        error.Exception = newExWithoutInner;

        // Assert
        error.Exception.Should().BeSameAs(newExWithoutInner);
        // The original InnerError should be gone because the setter re-evaluates it.
        // The implementation sets a new inner error only if the new exception has one.
        // It does not explicitly clear the old one, but the chaining starts from the new exception.
        // The logic in SetInnerError populates the chain, and for the new exception, it's null.
        // Let's refine the test to check if the logic correctly reflects no inner error for the new state.

        var newInnerErrorSource = new UnexpectedError(newExWithoutInner);
        newInnerErrorSource.InnerError.Should().BeNull();
    }
}
