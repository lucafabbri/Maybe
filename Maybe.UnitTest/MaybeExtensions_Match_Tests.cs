using FluentAssertions;
using Maybe;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains unit tests for the 'Match' (Unwrap) extension methods.
/// </summary>
public class MaybeExtensions_Match_Tests
{
    private record User(string Name);
    private class TestError : Error { }

    private static readonly User TestUser = new("Alice");
    private static readonly TestError TestErrorCustom = new();

    // --- Sync Functions ---
    private string OnSome(User u) => u.Name;
    private string OnNone(TestError e) => "Error";

    // --- Async Functions ---
    private Task<string> OnSomeAsync(User u) => Task.FromResult(u.Name);
    private Task<string> OnNoneAsync(TestError e) => Task.FromResult("Error");

    // --- Match (Sync -> Sync) ---

    [Fact]
    public void Match_OnSuccess_ShouldInvokeOnSome()
    {
        // Arrange
        Maybe<User, TestError> maybe = TestUser;

        // Act
        var result = maybe.Match(OnSome, OnNone);

        // Assert
        result.Should().Be("Alice");
    }

    [Fact]
    public void Match_OnError_ShouldInvokeOnNone()
    {
        // Arrange
        Maybe<User, TestError> maybe = TestErrorCustom;

        // Act
        var result = maybe.Match(OnSome, OnNone);

        // Assert
        result.Should().Be("Error");
    }

    // --- Match (Async -> Sync) ---

    [Fact]
    public async Task Match_OnSuccessTask_ShouldInvokeOnSome()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<User, TestError>)TestUser);

        // Act
        var result = await maybeTask.Match(OnSome, OnNone);

        // Assert
        result.Should().Be("Alice");
    }

    [Fact]
    public async Task Match_OnErrorTask_ShouldInvokeOnNone()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<User, TestError>)TestErrorCustom);

        // Act
        var result = await maybeTask.Match(OnSome, OnNone);

        // Assert
        result.Should().Be("Error");
    }

    // --- MatchAsync (All combinations) ---

    [Fact]
    public async Task MatchAsync_SyncMaybe_OnSuccess_ShouldInvokeAsyncOnSome()
    {
        Maybe<User, TestError> maybe = TestUser;
        (await maybe.MatchAsync(OnSomeAsync, OnNoneAsync)).Should().Be("Alice");
        (await maybe.MatchAsync(OnSomeAsync, OnNone)).Should().Be("Alice");
        (await maybe.MatchAsync(OnSome, OnNoneAsync)).Should().Be("Alice");
    }

    [Fact]
    public async Task MatchAsync_SyncMaybe_OnError_ShouldInvokeAsyncOnNone()
    {
        Maybe<User, TestError> maybe = TestErrorCustom;
        (await maybe.MatchAsync(OnSomeAsync, OnNoneAsync)).Should().Be("Error");
        (await maybe.MatchAsync(OnSomeAsync, OnNone)).Should().Be("Error");
        (await maybe.MatchAsync(OnSome, OnNoneAsync)).Should().Be("Error");
    }

    [Fact]
    public async Task MatchAsync_AsyncTask_OnSuccess_ShouldInvokeAsyncOnSome()
    {
        var maybeTask = Task.FromResult((Maybe<User, TestError>)TestUser);
        (await maybeTask.MatchAsync(OnSomeAsync, OnNoneAsync)).Should().Be("Alice");
        (await maybeTask.MatchAsync(OnSomeAsync, OnNone)).Should().Be("Alice");
        (await maybeTask.MatchAsync(OnSome, OnNoneAsync)).Should().Be("Alice");
    }

    [Fact]
    public async Task MatchAsync_AsyncTask_OnError_ShouldInvokeAsyncOnNone()
    {
        var maybeTask = Task.FromResult((Maybe<User, TestError>)TestErrorCustom);
        (await maybeTask.MatchAsync(OnSomeAsync, OnNoneAsync)).Should().Be("Error");
        (await maybeTask.MatchAsync(OnSomeAsync, OnNone)).Should().Be("Error");
        (await maybeTask.MatchAsync(OnSome, OnNoneAsync)).Should().Be("Error");
    }
}

