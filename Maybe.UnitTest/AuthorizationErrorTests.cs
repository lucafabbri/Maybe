using FluentAssertions;
using Maybe;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains unit tests for the AuthorizationError class.
/// </summary>
public class AuthorizationErrorTests
{
    [Fact]
    public void ParameterlessConstructor_ShouldInitializeWithDefaultUnauthorizedValues()
    {
        // Act
        var error = new AuthorizationError();

        // Assert
        error.Type.Should().Be(OutcomeType.Unauthorized);
        error.Code.Should().Be("Authorization.Unauthorized");
        error.Message.Should().Be("User 'anonymous' is not authorized to perform this action.");
        error.Action.Should().BeEmpty();
        error.UserId.Should().BeNull();
        error.ResourceIdentifier.Should().BeNull();
    }

    [Fact]
    public void ParameterizedConstructor_ShouldSetAllPropertiesCorrectly()
    {
        // Arrange
        var innerError = new Error();

        // Act
        var error = new AuthorizationError(
            OutcomeType.Forbidden,
            "DeleteUser",
            "user-123",
            "admin-456",
            "Admin access required.",
            "Auth.AdminOnly",
            innerError);

        // Assert
        error.Type.Should().Be(OutcomeType.Forbidden);
        error.Code.Should().Be("Auth.AdminOnly");
        error.Message.Should().Be("Admin access required.");
        error.Action.Should().Be("DeleteUser");
        error.ResourceIdentifier.Should().Be("user-123");
        error.UserId.Should().Be("admin-456");
        error.InnerError.Should().BeSameAs(innerError);
    }

    [Fact]
    public void Setters_ShouldAllowPropertyModification()
    {
        // Arrange
        var error = new AuthorizationError(OutcomeType.Forbidden, action: "EditPost", code: "Custom.Auth", message: "Custom Message");

        // Act
        error.UserId = "editor-007";
        error.ResourceIdentifier = "post-99";

        // Assert
        error.Action.Should().Be("EditPost");
        error.UserId.Should().Be("editor-007");
        error.ResourceIdentifier.Should().Be("post-99");
        error.Type.Should().Be(OutcomeType.Forbidden);
        error.Code.Should().Be("Custom.Auth");
        error.Message.Should().Be("Custom Message");
    }

    [Theory]
    [InlineData("user-1", "read", "doc-1", "User 'user-1' is not authorized to perform action 'read' on resource 'doc-1'.")]
    [InlineData(null, "read", "doc-1", "User 'anonymous' is not authorized to perform action 'read' on resource 'doc-1'.")]
    [InlineData("user-1", "export", null, "User 'user-1' is not authorized to perform action 'export'.")]
    [InlineData(null, "export", null, "User 'anonymous' is not authorized to perform action 'export'.")]
    [InlineData("user-1", "", null, "User 'user-1' is not authorized to perform this action.")]
    public void GenerateDefaultMessage_ShouldProduceCorrectMessageBasedOnInputs(string? userId, string action, string? resourceId, string expectedMessage)
    {
        // Act
        var error = new AuthorizationError(OutcomeType.Forbidden, action, resourceId, userId, null, null, null);

        // Assert
        error.Message.Should().Be(expectedMessage);
    }
}
