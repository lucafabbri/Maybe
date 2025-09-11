using Xunit;
using static Maybe.Tests.TestData;

namespace Maybe.Tests;

public class MaybeExtensions_Select_Tests
{
    private static readonly User TestUser = new(1, "Active User");

    [Fact]
    public void Select_WhenSuccess_ReturnsTransformedValue()
    {
        var maybeUser = TestUser.MightBe();
        var result = maybeUser.Select(u => u.Name);
        Assert.True(result.IsSuccess);
        Assert.Equal(TestUser.Name, result.ValueOrThrow());
    }

    [Fact]
    public void Select_WhenError_PropagatesError()
    {
        var maybeUser = TestError.MightBe<User,Error>();
        var result = maybeUser.Select(u => u.Name);
        Assert.True(result.IsError);
        Assert.Equal(TestError, result.ErrorOrThrow());
    }

    [Fact]
    public async Task Select_WhenTaskSuccess_ReturnsTransformedValue()
    {
        var maybeTask = Task.FromResult(TestUser.MightBe());
        var result = await maybeTask.Select(u => u.Name);
        Assert.True(result.IsSuccess);
        Assert.Equal(TestUser.Name, result.ValueOrThrow());
    }

    [Fact]
    public async Task Select_WhenTaskError_PropagatesError()
    {
        var maybeTask = Task.FromResult(TestError.MightBe<User>());
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
        Assert.Equal(TestError, result.ErrorOrThrow());
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
