using FluentAssertions;
using Maybe;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains unit tests for the ThenDo and ElseDo extension methods.
/// </summary>
public class MaybeExtensions_DoTests
{
    private class User { }
    private class TestError : Error { }

    #region ThenDo Tests

    [Fact]
    public void ThenDo_OnSuccess_ShouldExecuteAction()
    {
        // Arrange
        var maybe = Maybe<User, TestError>.Some(new User());
        var wasCalled = false;
        Action<User> action = u => wasCalled = true;

        // Act
        maybe.ThenDo(action);

        // Assert
        wasCalled.Should().BeTrue();
    }

    [Fact]
    public void ThenDo_OnError_ShouldNotExecuteAction()
    {
        // Arrange
        var maybe = Maybe<User, TestError>.None(new TestError());
        var wasCalled = false;
        Action<User> action = u => wasCalled = true;

        // Act
        maybe.ThenDo(action);

        // Assert
        wasCalled.Should().BeFalse();
    }

    [Fact]
    public async Task ThenDoAsync_OnSuccess_ShouldExecuteAction()
    {
        // Arrange
        var maybe = Maybe<User, TestError>.Some(new User());
        var wasCalled = false;
        Func<User, Task> action = u => { wasCalled = true; return Task.CompletedTask; };

        // Act
        await maybe.ThenDoAsync(action);

        // Assert
        wasCalled.Should().BeTrue();
    }

    [Fact]
    public async Task ThenDoAsync_OnError_ShouldNotExecuteAction()
    {
        // Arrange
        var maybe = Maybe<User, TestError>.None(new TestError());
        var wasCalled = false;
        Func<User, Task> action = u => { wasCalled = true; return Task.CompletedTask; };

        // Act
        await maybe.ThenDoAsync(action);

        // Assert
        wasCalled.Should().BeFalse();
    }

    [Fact]
    public async Task ThenDo_OnSuccessTask_ShouldExecuteAction()
    {
        // Arrange
        var maybeTask = Task.FromResult(Maybe<User, TestError>.Some(new User()));
        var wasCalled = false;
        Action<User> action = u => wasCalled = true;

        // Act
        await maybeTask.ThenDo(action);

        // Assert
        wasCalled.Should().BeTrue();
    }

    [Fact]
    public async Task ThenDoAsync_OnSuccessTask_ShouldExecuteAction()
    {
        // Arrange
        var maybeTask = Task.FromResult(Maybe<User, TestError>.Some(new User()));
        var wasCalled = false;
        Func<User, Task> action = u => { wasCalled = true; return Task.CompletedTask; };

        // Act
        await maybeTask.ThenDoAsync(action);

        // Assert
        wasCalled.Should().BeTrue();
    }

    #endregion

    #region ElseDo Tests

    [Fact]
    public void ElseDo_OnError_ShouldExecuteAction()
    {
        // Arrange
        var maybe = Maybe<User, TestError>.None(new TestError());
        var wasCalled = false;
        Action<TestError> action = e => wasCalled = true;

        // Act
        maybe.ElseDo(action);

        // Assert
        wasCalled.Should().BeTrue();
    }

    [Fact]
    public void ElseDo_OnSuccess_ShouldNotExecuteAction()
    {
        // Arrange
        var maybe = Maybe<User, TestError>.Some(new User());
        var wasCalled = false;
        Action<TestError> action = e => wasCalled = true;

        // Act
        maybe.ElseDo(action);

        // Assert
        wasCalled.Should().BeFalse();
    }

    [Fact]
    public async Task ElseDoAsync_OnError_ShouldExecuteAction()
    {
        // Arrange
        var maybe = Maybe<User, TestError>.None(new TestError());
        var wasCalled = false;
        Func<TestError, Task> action = e => { wasCalled = true; return Task.CompletedTask; };

        // Act
        await maybe.ElseDoAsync(action);

        // Assert
        wasCalled.Should().BeTrue();
    }

    [Fact]
    public async Task ElseDoAsync_OnSuccess_ShouldNotExecuteAction()
    {
        // Arrange
        var maybe = Maybe<User, TestError>.Some(new User());
        var wasCalled = false;
        Func<TestError, Task> action = e => { wasCalled = true; return Task.CompletedTask; };

        // Act
        await maybe.ElseDoAsync(action);

        // Assert
        wasCalled.Should().BeFalse();
    }

    [Fact]
    public async Task ElseDo_OnErrorTask_ShouldExecuteAction()
    {
        // Arrange
        var maybeTask = Task.FromResult(Maybe<User, TestError>.None(new TestError()));
        var wasCalled = false;
        Action<TestError> action = e => wasCalled = true;

        // Act
        await maybeTask.ElseDo(action);

        // Assert
        wasCalled.Should().BeTrue();
    }

    [Fact]
    public async Task ElseDoAsync_OnErrorTask_ShouldExecuteAction()
    {
        // Arrange
        var maybeTask = Task.FromResult(Maybe<User, TestError>.None(new TestError()));
        var wasCalled = false;
        Func<TestError, Task> action = e => { wasCalled = true; return Task.CompletedTask; };

        // Act
        await maybeTask.ElseDoAsync(action);

        // Assert
        wasCalled.Should().BeTrue();
    }

    #endregion
}
