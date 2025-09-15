using FluentAssertions;
using Maybe;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains unit tests for the 'Else' (Fallback) extension methods.
/// </summary>
public class MaybeExtensions_Else_Tests
{
    private record User(string Name);
    private class TestError : Error { }

    private static readonly User SuccessUser = new("Alice");
    private static readonly User FallbackUser = new("Guest");
    private static readonly TestError TestErrorCustom = new();

    // --- Else (Sync Fallback) ---

    [Fact]
    public void Else_WithValue_OnSuccess_ReturnsOriginalValue()
    {
        // Arrange
        Maybe<User, TestError> maybe = SuccessUser;

        // Act
        var result = maybe.Else(FallbackUser);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(SuccessUser);
    }

    [Fact]
    public void Else_WithValue_OnError_ReturnsFallbackValue()
    {
        // Arrange
        Maybe<User, TestError> maybe = TestErrorCustom;

        // Act
        var result = maybe.Else(FallbackUser);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(FallbackUser);
    }

    [Fact]
    public void Else_WithFunc_OnSuccess_ReturnsOriginalValueAndDoesNotInvokeFunc()
    {
        // Arrange
        Maybe<User, TestError> maybe = SuccessUser;
        var wasCalled = false;
        Func<TestError, User> fallbackFunc = e => { wasCalled = true; return FallbackUser; };

        // Act
        var result = maybe.Else(fallbackFunc);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(SuccessUser);
        wasCalled.Should().BeFalse();
    }

    [Fact]
    public void Else_WithFunc_OnError_InvokesFuncAndReturnsItsValue()
    {
        // Arrange
        Maybe<User, TestError> maybe = TestErrorCustom;
        var wasCalled = false;
        Func<TestError, User> fallbackFunc = e => { wasCalled = true; return FallbackUser; };

        // Act
        var result = maybe.Else(fallbackFunc);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(FallbackUser);
        wasCalled.Should().BeTrue();
    }

    // --- Else on Task<Maybe> (Sync Fallback) ---

    [Fact]
    public async Task Else_OnSuccessTask_ReturnsOriginalValue()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<User, TestError>)SuccessUser);

        // Act
        var resultWithValue = await maybeTask.Else(FallbackUser);
        var resultWithFunc = await maybeTask.Else(e => FallbackUser);

        // Assert
        resultWithValue.IsSuccess.Should().BeTrue();
        resultWithValue.ValueOrThrow().Should().Be(SuccessUser);
        resultWithFunc.IsSuccess.Should().BeTrue();
        resultWithFunc.ValueOrThrow().Should().Be(SuccessUser);
    }

    [Fact]
    public async Task Else_OnErrorTask_ReturnsFallbackValue()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<User, TestError>)TestErrorCustom);

        // Act
        var resultWithValue = await maybeTask.Else(FallbackUser);
        var resultWithFunc = await maybeTask.Else(e => FallbackUser);

        // Assert
        resultWithValue.IsSuccess.Should().BeTrue();
        resultWithValue.ValueOrThrow().Should().Be(FallbackUser);
        resultWithFunc.IsSuccess.Should().BeTrue();
        resultWithFunc.ValueOrThrow().Should().Be(FallbackUser);
    }

    // --- ElseAsync (Async Fallback) ---

    [Fact]
    public async Task ElseAsync_OnSuccess_ReturnsOriginalValueAndDoesNotInvokeFunc()
    {
        // Arrange
        Maybe<User, TestError> maybe = SuccessUser;
        var wasCalled = false;
        Func<TestError, Task<User>> fallbackFunc = e => { wasCalled = true; return Task.FromResult(FallbackUser); };

        // Act
        var result = await maybe.ElseAsync(fallbackFunc);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(SuccessUser);
        wasCalled.Should().BeFalse();
    }

    [Fact]
    public async Task ElseAsync_OnError_InvokesFuncAndReturnsItsValue()
    {
        // Arrange
        Maybe<User, TestError> maybe = TestErrorCustom;
        var wasCalled = false;
        Func<TestError, Task<User>> fallbackFunc = e => { wasCalled = true; return Task.FromResult(FallbackUser); };

        // Act
        var result = await maybe.ElseAsync(fallbackFunc);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(FallbackUser);
        wasCalled.Should().BeTrue();
    }

    // --- ElseAsync on Task<Maybe> (Async Fallback) ---

    [Fact]
    public async Task ElseAsync_OnSuccessTask_ReturnsOriginalValue()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<User, TestError>)SuccessUser);
        var wasCalled = false;
        Func<TestError, Task<User>> fallbackFunc = e => { wasCalled = true; return Task.FromResult(FallbackUser); };

        // Act
        var result = await maybeTask.ElseAsync(fallbackFunc);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(SuccessUser);
        wasCalled.Should().BeFalse();
    }

    [Fact]
    public async Task ElseAsync_OnErrorTask_ReturnsFallbackValue()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<User, TestError>)TestErrorCustom);
        var wasCalled = false;
        Func<TestError, Task<User>> fallbackFunc = e => { wasCalled = true; return Task.FromResult(FallbackUser); };

        // Act
        var result = await maybeTask.ElseAsync(fallbackFunc);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be(FallbackUser);
        wasCalled.Should().BeTrue();
    }
}
