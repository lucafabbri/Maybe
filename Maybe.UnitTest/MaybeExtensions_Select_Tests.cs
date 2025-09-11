using Xunit;
using static Maybe.Tests.TestData;

namespace Maybe.Tests;

public class MaybeExtensions_Select_Tests
{
    private static readonly User TestUser = new(1, "Active User");

    [Fact]
    public void Select_WhenSuccess_ReturnsTransformedValue()
    {
        Maybe<User, Error> maybeUser = TestUser;
        var result = maybeUser.Select(u => u.Name);
        Assert.True(result.IsSuccess);
        Assert.Equal(TestUser.Name, result.ValueOrThrow());
    }

    [Fact]
    public void Select_WhenError_PropagatesError()
    {
        Maybe<User, Error> maybeUser = TestError;
        var result = maybeUser.Select(u => u.Name);
        Assert.True(result.IsError);
        Assert.Equal(TestError, result.ErrorOrThrow());
    }

    [Fact]
    public async Task Select_WhenTaskSuccess_ReturnsTransformedValue()
    {
        var maybeTask = Task.FromResult((Maybe<User, Error>)TestUser);
        var result = await maybeTask.Select(u => u.Name);
        Assert.True(result.IsSuccess);
        Assert.Equal(TestUser.Name, result.ValueOrThrow());
    }

    [Fact]
    public async Task Select_WhenTaskError_PropagatesError()
    {
        var maybeTask = Task.FromResult((Maybe<User, Error>)TestError);
        var result = await maybeTask.Select(u => u.Name);
        Assert.True(result.IsError);
        Assert.Equal(TestError, result.ErrorOrThrow());
    }

    // New Tests for Maybe<TValue>

    [Fact]
    public void Select_WhenMaybeOfT_Success_ReturnsTransformedValue()
    {
        Maybe<User> maybeUser = TestUser;
        var result = maybeUser.Select(u => u.Name);
        Assert.True(result.IsSuccess);
        Assert.Equal(TestUser.Name, result.ValueOrThrow());
    }

    [Fact]
    public void Select_WhenMaybeOfT_Error_PropagatesError()
    {
        Maybe<User> maybeUser = TestError;
        var result = maybeUser.Select(u => u.Name);
        Assert.True(result.IsError);
        Assert.Equal(TestError, (Error)result.ErrorOrThrow());
    }

    [Fact]
    public async Task Select_WhenMaybeOfT_TaskSuccess_ReturnsTransformedValue()
    {
        var maybeTask = Task.FromResult((Maybe<User>)TestUser);
        var result = await maybeTask.Select(u => u.Name);
        Assert.True(result.IsSuccess);
        Assert.Equal(TestUser.Name, result.ValueOrThrow());
    }
}
