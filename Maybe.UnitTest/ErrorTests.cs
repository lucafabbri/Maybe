using FluentAssertions;
using Maybe;
using System;
using System.Collections.Generic;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains unit tests for the Error class and its specialized implementations.
/// </summary>
public class ErrorTests
{
    [Fact]
    public void Error_ParameterlessConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var error = new Error();

        // Assert
        error.Type.Should().Be(OutcomeType.Failure);
        error.Code.Should().Be("Default.Error");
        error.Message.Should().Be("An error has occurred.");
    }

    [Fact]
    public void Error_ParameterizedConstructor_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var inner = new Error();

        // Act
        var error = new Error(OutcomeType.Conflict, "Test.Code", "A test message.", inner);

        // Assert
        error.Type.Should().Be(OutcomeType.Conflict);
        error.Code.Should().Be("Test.Code");
        error.Message.Should().Be("A test message.");
        error.InnerError.Should().BeSameAs(inner);
    }

    [Fact]
    public void Custom_Factory_ShouldCreateError()
    {
        // Act
        var error = Error.Custom(OutcomeType.Failure, "Custom.Code", "Custom message.");

        // Assert
        error.Should().BeOfType<Error>();
        error.Type.Should().Be(OutcomeType.Failure);
        error.Code.Should().Be("Custom.Code");
        error.Message.Should().Be("Custom message.");
    }

    [Fact]
    public void Failure_Factory_ShouldCreateFailureErrorWithCorrectData()
    {
        // Arrange
        var context = new Dictionary<string, object> { { "Key", "Value" } };

        // Act
        var error = Error.Failure("A specific failure", "My.Failure", context);

        // Assert
        error.Should().BeOfType<FailureError>();
        error.Type.Should().Be(OutcomeType.Failure);
        error.Message.Should().Be("A specific failure");
        error.Code.Should().Be("My.Failure");

        var failureError = error;
        failureError!.ContextData.Should().ContainKey("Key").And.ContainValue("Value");
    }

    [Fact]
    public void Unexpected_Factory_ShouldCreateUnexpectedErrorWrappingException()
    {
        // Arrange
        var innerEx = new InvalidOperationException("Inner");
        var ex = new Exception("Outer", innerEx);

        // Act
        var error = Error.Unexpected(ex, "Custom message", "My.Exception");

        // Assert
        error.Should().BeOfType<UnexpectedError>();
        error.Type.Should().Be(OutcomeType.Unexpected);
        error.Message.Should().Be("Custom message");
        error.Code.Should().Be("My.Exception");

        var unexpectedError = error;
        unexpectedError!.Exception.Should().BeSameAs(ex);
        unexpectedError.InnerError.Should().NotBeNull();
        unexpectedError.InnerError.Should().BeOfType<UnexpectedError>();
        (unexpectedError.InnerError as UnexpectedError)!.Exception.Should().BeSameAs(innerEx);
    }

    [Fact]
    public void Unexpected_Factory_WithMessageAndCodeNull_ShouldUseExceptionDefaults()
    {
        // Arrange
        var ex = new Exception("Exception message");

        // Act
        var error = Error.Unexpected(ex);

        // Assert
        error.Message.Should().Be("Exception message");
        error.Code.Should().Be("System.Exception");
    }

    [Fact]
    public void Validation_Factory_ShouldCreateValidationError()
    {
        // Arrange
        var fieldErrors = new Dictionary<string, string> { { "Email", "Is required" } };

        // Act
        var error = Error.Validation(fieldErrors, "Validation failed", "My.Validation");

        // Assert
        error.Should().BeOfType<ValidationError>();
        error.Type.Should().Be(OutcomeType.Validation);
        error.Message.Should().Be("Validation failed");
        error.Code.Should().Be("My.Validation");
        error!.FieldErrors.Should().ContainKey("Email");
    }

    [Fact]
    public void Conflict_Factory_ShouldCreateConflictError()
    {
        // Arrange
        var conflictingParams = new Dictionary<string, object> { { "Id", 123 } };

        // Act
        var error = Error.Conflict(ConflictType.Duplicate, "User", conflictingParams, "User exists", "My.Conflict");

        // Assert
        error.Should().BeOfType<ConflictError>();
        error.Type.Should().Be(OutcomeType.Conflict);
        error!.ConflictType.Should().Be(ConflictType.Duplicate);
        error!.ResourceType.Should().Be("User");
        error!.ConflictingParameters.Should().ContainKey("Id");
    }

    [Fact]
    public void NotFound_Factory_ShouldCreateNotFoundError()
    {
        // Act
        var error = Error.NotFound("Product", "SKU123", "Product not in stock", "My.NotFound");

        // Assert
        error.Should().BeOfType<NotFoundError>();
        error.Type.Should().Be(OutcomeType.NotFound);
        error.Message.Should().Be("Product not in stock");
        error.Code.Should().Be("My.NotFound");
        error!.EntityName.Should().Be("Product");
        error!.Identifier.Should().Be("SKU123");
    }

    [Theory]
    [InlineData(OutcomeType.Unauthorized)]
    [InlineData(OutcomeType.Forbidden)]
    public void Authorization_Factories_ShouldCreateAuthorizationError(OutcomeType outcomeType)
    {
        // Arrange
        Func<string, string?, string?, string?, string?, Error?, AuthorizationError> factory =
            outcomeType == OutcomeType.Unauthorized ? Error.Unauthorized : Error.Forbidden;

        // Act
        var error = factory("Delete", "res-123", "user-456", "Not allowed", "My.Auth", null);

        // Assert
        error.Should().BeOfType<AuthorizationError>();
        error.Type.Should().Be(outcomeType);
        error.Message.Should().Be("Not allowed");
        error.Code.Should().Be("My.Auth");
        error!.Action.Should().Be("Delete");
        error!.ResourceIdentifier.Should().Be("res-123");
        error!.UserId.Should().Be("user-456");
    }

    [Fact]
    public void Authorization_Factory_WithoutResource_ShouldGenerateCorrectDefaultMessage()
    {
        // Act
        var error = Error.Forbidden("ExportData", userId: "user-789");

        // Assert
        var expectedMessage = "User 'user-789' is not authorized to perform action 'ExportData'.";
        error.Message.Should().Be(expectedMessage);
    }
}
