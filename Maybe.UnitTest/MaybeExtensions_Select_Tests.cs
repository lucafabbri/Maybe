using FluentAssertions;
using Maybe;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains unit tests for the 'Select' (Map) extension methods.
/// </summary>
public class MaybeExtensions_Select_Tests
{
    private record User(string Name);
    private class TestError : Error { }

    private static readonly User TestUser = new("Alice");
    private static readonly TestError TestErrorCustom = new();

    // --- Select (Sync -> Sync) ---

    [Fact]
    public void Select_OnSuccess_ShouldTransformValue()
    {
        // Arrange
        Maybe<User, TestError> maybe = TestUser;

        // Act
        var result = maybe.Select(u => u.Name);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be("Alice");
    }

    [Fact]
    public void Select_OnError_ShouldPropagateError()
    {
        // Arrange
        Maybe<User, TestError> maybe = TestErrorCustom;
        var wasCalled = false;
        Func<User, string> selector = u => { wasCalled = true; return u.Name; };

        // Act
        var result = maybe.Select(selector);

        // Assert
        result.IsError.Should().BeTrue();
        result.ErrorOrThrow().Should().Be(TestErrorCustom);
        wasCalled.Should().BeFalse();
    }

    // --- Select (Async -> Sync) ---

    [Fact]
    public async Task Select_OnSuccessTask_ShouldTransformValue()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<User, TestError>)TestUser);

        // Act
        var result = await maybeTask.Select(u => u.Name);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be("Alice");
    }

    [Fact]
    public async Task Select_OnErrorTask_ShouldPropagateError()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<User, TestError>)TestErrorCustom);
        var wasCalled = false;
        Func<User, string> selector = u => { wasCalled = true; return u.Name; };

        // Act
        var result = await maybeTask.Select(selector);

        // Assert
        result.IsError.Should().BeTrue();
        result.ErrorOrThrow().Should().Be(TestErrorCustom);
        wasCalled.Should().BeFalse();
    }

    // --- SelectAsync (Sync -> Async) ---

    [Fact]
    public async Task SelectAsync_OnSuccess_ShouldTransformValue()
    {
        // Arrange
        Maybe<User, TestError> maybe = TestUser;

        // Act
        var result = await maybe.SelectAsync(u => Task.FromResult(u.Name));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be("Alice");
    }

    [Fact]
    public async Task SelectAsync_OnError_ShouldPropagateError()
    {
        // Arrange
        Maybe<User, TestError> maybe = TestErrorCustom;
        var wasCalled = false;
        Func<User, Task<string>> selector = u => { wasCalled = true; return Task.FromResult(u.Name); };

        // Act
        var result = await maybe.SelectAsync(selector);

        // Assert
        result.IsError.Should().BeTrue();
        result.ErrorOrThrow().Should().Be(TestErrorCustom);
        wasCalled.Should().BeFalse();
    }

    // --- SelectAsync (Async -> Async) ---

    [Fact]
    public async Task SelectAsync_OnSuccessTask_ShouldTransformValue()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<User, TestError>)TestUser);

        // Act
        var result = await maybeTask.SelectAsync(u => Task.FromResult(u.Name));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be("Alice");
    }

    [Fact]
    public async Task SelectAsync_OnErrorTask_ShouldPropagateError()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<User, TestError>)TestErrorCustom);
        var wasCalled = false;
        Func<User, Task<string>> selector = u => { wasCalled = true; return Task.FromResult(u.Name); };

        // Act
        var result = await maybeTask.SelectAsync(selector);

        // Assert
        result.IsError.Should().BeTrue();
        result.ErrorOrThrow().Should().Be(TestErrorCustom);
        wasCalled.Should().BeFalse();
    }
}
