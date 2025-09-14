using FluentAssertions;
using Maybe;
using System;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains unit tests for the Maybe<TValue, TError> struct.
/// </summary>
public class MaybeTests
{
    private class User { public string Name { get; set; } = "Test"; }
    private class TestOutcome : IOutcome { public OutcomeType Type => OutcomeType.Created; }
    private class MyCustomError : Error
    {
        public MyCustomError() { }
        public MyCustomError(BaseError baseError) { Message = baseError.Message; } // For Activator
    }

    [Fact]
    public void Some_ShouldCreateSuccessMaybe()
    {
        // Arrange
        var user = new User();

        // Act
        var maybe = Maybe<User, Error>.Some(user);

        // Assert
        maybe.IsSuccess.Should().BeTrue();
        maybe.IsError.Should().BeFalse();
        maybe.ValueOrThrow().Should().BeSameAs(user);
    }

    [Fact]
    public void None_ShouldCreateErrorMaybe()
    {
        // Arrange
        var error = new Error();

        // Act
        var maybe = Maybe<User, Error>.None(error);

        // Assert
        maybe.IsSuccess.Should().BeFalse();
        maybe.IsError.Should().BeTrue();
        maybe.ErrorOrThrow().Should().BeSameAs(error);
    }

    [Fact]
    public void ImplicitConversion_FromValue_ShouldCreateSuccessMaybe()
    {
        // Arrange
        var user = new User();

        // Act
        Maybe<User, Error> maybe = user;

        // Assert
        maybe.IsSuccess.Should().BeTrue();
        maybe.ValueOrThrow().Should().BeSameAs(user);
    }

    [Fact]
    public void ImplicitConversion_FromError_ShouldCreateErrorMaybe()
    {
        // Arrange
        var error = new Error();

        // Act
        Maybe<User, Error> maybe = error;

        // Assert
        maybe.IsError.Should().BeTrue();
        maybe.ErrorOrThrow().Should().BeSameAs(error);
    }

    [Fact]
    public void None_WithBaseError_WhenErrorIsCorrectType_ShouldCreateErrorMaybe()
    {
        // Arrange
        var customError = new MyCustomError();
        BaseError baseError = customError;

        // Act
        var maybe = Maybe<User, MyCustomError>.None(baseError);

        // Assert
        maybe.IsError.Should().BeTrue();
        maybe.ErrorOrThrow().Should().BeSameAs(customError);
    }

    [Fact]
    public void None_WithBaseError_WhenErrorIsNotCorrectType_ShouldCreateNewErrorViaActivator()
    {
        // Arrange
        var notFoundError = new NotFoundError(); // This is a BaseError, but not MyCustomError
        BaseError baseError = notFoundError;

        // Act
        var maybe = Maybe<User, MyCustomError>.None(baseError);

        // Assert
        maybe.IsError.Should().BeTrue();
        maybe.ErrorOrThrow().Should().BeOfType<MyCustomError>();
        maybe.ErrorOrThrow().Message.Should().Be(notFoundError.Message);
    }

    [Fact]
    public void ValueOrThrow_OnSuccess_ReturnsValue()
    {
        // Arrange
        var user = new User();
        Maybe<User, Error> maybe = user;

        // Act & Assert
        maybe.ValueOrThrow().Should().BeSameAs(user);
    }

    [Fact]
    public void ValueOrThrow_OnError_ThrowsInvalidOperationException()
    {
        // Arrange
        Maybe<User, Error> maybe = new Error();

        // Act
        Action act = () => maybe.ValueOrThrow("Custom message");

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Custom message");
    }

    [Fact]
    public void ErrorOrThrow_OnError_ReturnsError()
    {
        // Arrange
        var error = new Error();
        Maybe<User, Error> maybe = error;

        // Act & Assert
        maybe.ErrorOrThrow().Should().BeSameAs(error);
    }

    [Fact]
    public void ErrorOrThrow_OnSuccess_ThrowsInvalidOperationException()
    {
        // Arrange
        Maybe<User, Error> maybe = new User();

        // Act
        Action act = () => maybe.ErrorOrThrow();

        // Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Cannot access the error of a success outcome.");
    }

    [Fact]
    public void ValueOrDefault_OnSuccess_ReturnsValue()
    {
        // Arrange
        var user = new User();
        Maybe<User, Error> maybe = user;

        // Act & Assert
        maybe.ValueOrDefault().Should().BeSameAs(user);
    }

    [Fact]
    public void ValueOrDefault_OnError_ReturnsDefault()
    {
        // Arrange
        Maybe<User, Error> maybeUser = new Error();
        Maybe<int, Error> maybeInt = new Error();

        // Act & Assert
        maybeUser.ValueOrDefault().Should().BeNull();
        maybeInt.ValueOrDefault().Should().Be(0);
    }

    [Fact]
    public void ErrorOrDefault_OnError_ReturnsError()
    {
        // Arrange
        var error = new Error();
        Maybe<User, Error> maybe = error;

        // Act & Assert
        maybe.ErrorOrDefault().Should().BeSameAs(error);
    }

    [Fact]
    public void ErrorOrDefault_OnSuccess_ReturnsDefault()
    {
        // Arrange
        Maybe<User, Error> maybe = new User();

        // Act & Assert
        maybe.ErrorOrDefault().Should().BeNull();
    }

    [Fact]
    public void Type_OnSuccess_WithValueNotImplementingIOutcome_ReturnsSuccess()
    {
        // Arrange
        Maybe<User, Error> maybe = new User();

        // Act & Assert
        maybe.Type.Should().Be(OutcomeType.Success);
    }

    [Fact]
    public void Type_OnSuccess_WithValueImplementingIOutcome_ReturnsValueType()
    {
        // Arrange
        Maybe<TestOutcome, Error> maybe = new TestOutcome();

        // Act & Assert
        maybe.Type.Should().Be(OutcomeType.Created);
    }

    [Fact]
    public void Type_OnError_ReturnsErrorType()
    {
        // Arrange
        Maybe<User, Error> maybe = new ConflictError();

        // Act & Assert
        maybe.Type.Should().Be(OutcomeType.Conflict);
    }
}
