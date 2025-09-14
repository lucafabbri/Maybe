using FluentAssertions;
using Maybe;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains unit tests for the IfSome and IfNone extension methods.
/// </summary>
public class MaybeExtensions_IfTests
{
    private class User { }
    private class TestError : Error { }

    #region IfSome Tests

    [Fact]
    public void IfSome_OnSuccess_ShouldExecuteActionAndReturnOriginalMaybe()
    {
        // Arrange
        var user = new User();
        var maybe = Maybe<User, TestError>.Some(user);
        var wasCalled = false;
        Action<User> action = u => wasCalled = true;

        // Act
        var result = maybe.IfSome(action);

        // Assert
        wasCalled.Should().BeTrue();
        result.Should().Be(maybe);
    }

    [Fact]
    public void IfSome_OnError_ShouldNotExecuteActionAndReturnOriginalMaybe()
    {
        // Arrange
        var maybe = Maybe<User, TestError>.None(new TestError());
        var wasCalled = false;
        Action<User> action = u => wasCalled = true;

        // Act
        var result = maybe.IfSome(action);

        // Assert
        wasCalled.Should().BeFalse();
        result.Should().Be(maybe);
    }

    [Fact]
    public async Task IfSomeAsync_OnSuccess_ShouldExecuteActionAndReturnOriginalMaybe()
    {
        // Arrange
        var user = new User();
        var maybe = Maybe<User, TestError>.Some(user);
        var wasCalled = false;
        Func<User, Task> action = u => { wasCalled = true; return Task.CompletedTask; };

        // Act
        var result = await maybe.IfSomeAsync(action);

        // Assert
        wasCalled.Should().BeTrue();
        result.Should().Be(maybe);
    }

    [Fact]
    public async Task IfSomeAsync_OnError_ShouldNotExecuteActionAndReturnOriginalMaybe()
    {
        // Arrange
        var maybe = Maybe<User, TestError>.None(new TestError());
        var wasCalled = false;
        Func<User, Task> action = u => { wasCalled = true; return Task.CompletedTask; };

        // Act
        var result = await maybe.IfSomeAsync(action);

        // Assert
        wasCalled.Should().BeFalse();
        result.Should().Be(maybe);
    }

    [Fact]
    public async Task IfSome_OnSuccessTask_ShouldExecuteActionAndReturnMaybe()
    {
        // Arrange
        var maybeTask = Task.FromResult(Maybe<User, TestError>.Some(new User()));
        var wasCalled = false;
        Action<User> action = u => wasCalled = true;

        // Act
        var result = await maybeTask.IfSome(action);

        // Assert
        wasCalled.Should().BeTrue();
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task IfSomeAsync_OnSuccessTask_ShouldExecuteActionAndReturnMaybe()
    {
        // Arrange
        var maybeTask = Task.FromResult(Maybe<User, TestError>.Some(new User()));
        var wasCalled = false;
        Func<User, Task> action = u => { wasCalled = true; return Task.CompletedTask; };

        // Act
        var result = await maybeTask.IfSomeAsync(action);

        // Assert
        wasCalled.Should().BeTrue();
        result.IsSuccess.Should().BeTrue();
    }

    #endregion

    #region IfNone Tests

    [Fact]
    public void IfNone_OnError_ShouldExecuteActionAndReturnOriginalMaybe()
    {
        // Arrange
        var error = new TestError();
        var maybe = Maybe<User, TestError>.None(error);
        var wasCalled = false;
        Action<TestError> action = e => wasCalled = true;

        // Act
        var result = maybe.IfNone(action);

        // Assert
        wasCalled.Should().BeTrue();
        result.Should().Be(maybe);
    }

    [Fact]
    public void IfNone_OnSuccess_ShouldNotExecuteActionAndReturnOriginalMaybe()
    {
        // Arrange
        var maybe = Maybe<User, TestError>.Some(new User());
        var wasCalled = false;
        Action<TestError> action = e => wasCalled = true;

        // Act
        var result = maybe.IfNone(action);

        // Assert
        wasCalled.Should().BeFalse();
        result.Should().Be(maybe);
    }

    [Fact]
    public async Task IfNoneAsync_OnError_ShouldExecuteActionAndReturnOriginalMaybe()
    {
        // Arrange
        var error = new TestError();
        var maybe = Maybe<User, TestError>.None(error);
        var wasCalled = false;
        Func<TestError, Task> action = e => { wasCalled = true; return Task.CompletedTask; };

        // Act
        var result = await maybe.IfNoneAsync(action);

        // Assert
        wasCalled.Should().BeTrue();
        result.Should().Be(maybe);
    }

    [Fact]
    public async Task IfNoneAsync_OnSuccess_ShouldNotExecuteActionAndReturnOriginalMaybe()
    {
        // Arrange
        var maybe = Maybe<User, TestError>.Some(new User());
        var wasCalled = false;
        Func<TestError, Task> action = e => { wasCalled = true; return Task.CompletedTask; };

        // Act
        var result = await maybe.IfNoneAsync(action);

        // Assert
        wasCalled.Should().BeFalse();
        result.Should().Be(maybe);
    }

    [Fact]
    public async Task IfNone_OnErrorTask_ShouldExecuteActionAndReturnMaybe()
    {
        // Arrange
        var maybeTask = Task.FromResult(Maybe<User, TestError>.None(new TestError()));
        var wasCalled = false;
        Action<TestError> action = e => wasCalled = true;

        // Act
        var result = await maybeTask.IfNone(action);

        // Assert
        wasCalled.Should().BeTrue();
        result.IsError.Should().BeTrue();
    }

    [Fact]
    public async Task IfNoneAsync_OnErrorTask_ShouldExecuteActionAndReturnMaybe()
    {
        // Arrange
        var maybeTask = Task.FromResult(Maybe<User, TestError>.None(new TestError()));
        var wasCalled = false;
        Func<TestError, Task> action = e => { wasCalled = true; return Task.CompletedTask; };

        // Act
        var result = await maybeTask.IfNoneAsync(action);

        // Assert
        wasCalled.Should().BeTrue();
        result.IsError.Should().BeTrue();
    }

    #endregion
}
