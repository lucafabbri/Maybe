using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Maybe.Tests;

public class MaybeStructTests
{
    // --- Test Data ---
    private record TestValue(int Id);
    private class TestCustomError : Error
    {
        public string Detail { get; }

        public TestCustomError(string detail) : base(OutcomeType.Failure, "Test.Custom", "A custom test error.") { 
            Detail = detail;
        }
    }

    // --- Creation and State Tests ---

    [Fact]
    public void Some_WithValue_ShouldBeSuccess()
    {
        // Arrange
        var value = new TestValue(1);

        // Act
        var maybe = Maybe<TestValue, Error>.Some(value);

        // Assert
        Assert.True(maybe.IsSuccess);
        Assert.False(maybe.IsError);
        Assert.Equal(value, maybe.ValueOrThrow());
    }

    [Fact]
    public void None_WithError_ShouldBeError()
    {
        // Arrange
        var error = Error.NotFound();

        // Act
        var maybe = Maybe<TestValue, Error>.None(error);

        // Assert
        Assert.False(maybe.IsSuccess);
        Assert.True(maybe.IsError);
        Assert.Equal(error, maybe.ErrorOrThrow());
    }

    [Fact]
    public void ImplicitConversion_FromValue_ShouldBeSuccess()
    {
        // Arrange
        var value = new TestValue(1);

        // Act
        Maybe<TestValue, Error> maybe = value;

        // Assert
        Assert.True(maybe.IsSuccess);
        Assert.Equal(value, maybe.ValueOrThrow());
    }

    [Fact]
    public void ImplicitConversion_FromError_ShouldBeError()
    {
        // Arrange
        var error = Error.Conflict();

        // Act
        Maybe<TestValue, Error> maybe = error;

        // Assert
        Assert.True(maybe.IsError);
        Assert.Equal(error, maybe.ErrorOrThrow());
    }

    // --- OutcomeType Property Tests ---

    [Fact]
    public void Type_WhenSuccessWithNonIOutcomeValue_ShouldBeSuccess()
    {
        // Arrange
        Maybe<TestValue, Error> maybe = new TestValue(1);

        // Act & Assert
        Assert.Equal(OutcomeType.Success, maybe.Type);
    }

    [Fact]
    public void Type_WhenSuccessWithIOutcomeValue_ShouldReflectValueType()
    {
        // Arrange
        Maybe<Created, Error> maybe = Outcomes.Created;

        // Act & Assert
        Assert.Equal(OutcomeType.Created, maybe.Type);
    }

    [Fact]
    public void Type_WhenError_ShouldReflectErrorType()
    {
        // Arrange
        Maybe<TestValue, Error> maybe = Error.Forbidden();

        // Act & Assert
        Assert.Equal(OutcomeType.Forbidden, maybe.Type);
    }

    // --- Unsafe Accessor Tests ---

    [Fact]
    public void ValueOrThrow_WhenError_ShouldThrow()
    {
        // Arrange
        Maybe<TestValue, Error> maybe = Error.Validation();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => maybe.ValueOrThrow());
    }

    [Fact]
    public void ErrorOrThrow_WhenSuccess_ShouldThrow()
    {
        // Arrange
        Maybe<TestValue, Error> maybe = new TestValue(1);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => maybe.ErrorOrThrow());
    }

    // --- Safe Fallback Tests ---

    [Fact]
    public void ValueOrDefault_WhenSuccess_ShouldReturnValue()
    {
        // Arrange
        var value = new TestValue(1);
        Maybe<TestValue, Error> maybe = value;

        // Act & Assert
        Assert.Equal(value, maybe.ValueOrDefault());
    }

    [Fact]
    public void ValueOrDefault_WhenError_ShouldReturnDefault()
    {
        // Arrange
        Maybe<TestValue, Error> maybe = Error.Failure();

        // Act & Assert
        Assert.Null(maybe.ValueOrDefault()); // TestValue is a reference type
    }
}
