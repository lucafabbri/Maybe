using static Maybe.Tests.TestData;

namespace Maybe.Tests;

public class MaybeExtensions_Else_Tests
{
    private static readonly User TestUser = new(1, "Active User");
    private static readonly User InactiveUser = new(0, "Inactive User");
    [Fact]
    public void Else_WhenSuccess_ReturnsValue()
    {
        Maybe<User, TestCustomError> maybeUser = TestUser;
        var result = maybeUser.Else(InactiveUser);

        Assert.Equal(TestUser, result);
    }

    [Fact]
    public void Else_WhenError_ReturnsFallbackValue()
    {
        Maybe<User, FailureError> maybeUser = TestError;
        var result = maybeUser.Else(InactiveUser);

        Assert.Equal(InactiveUser, result);
    }

    [Fact]
    public void Else_WhenError_InvokesFallbackFunc()
    {
        Maybe<User, FailureError> maybeUser = TestError;
        var result = maybeUser.Else(e => InactiveUser);

        Assert.Equal(InactiveUser, result);
    }

    [Fact]
    public async Task Else_WhenTaskError_ReturnsFallbackValue()
    {
        var maybeTask = Task.FromResult((Maybe<User, FailureError>)TestError);
        var result = await maybeTask.Else(InactiveUser);

        Assert.Equal(InactiveUser, result);
    }
}