using static Maybe.Tests.TestData;

namespace Maybe.Tests;

public class MaybeExtensions_Match_Tests
{
    private static readonly User TestUser = new(1, "Active User");
    private static readonly Error TestError = Error.Failure("User.NotFound", "The user was not found.");
    [Fact]
    public void Match_WhenSuccess_InvokesOnSome()
    {
        Maybe<User> maybeUser = TestUser;
        var result = maybeUser.Match(
            onSome: u => u.Name,
            onNone: e => "Error");

        Assert.Equal(TestUser.Name, result);
    }

    [Fact]
    public void Match_WhenError_InvokesOnNone()
    {
        Maybe<User> maybeUser = TestError;
        var result = maybeUser.Match(
            onSome: u => "Success",
            onNone: e => e.Message);

        Assert.Equal(TestError.Message, result);
    }

    [Fact]
    public async Task Match_WhenTaskSuccess_InvokesOnSome()
    {
        var maybeTask = Task.FromResult((Maybe<User>)TestUser);
        var result = await maybeTask.Match(
            onSome: u => u.Name,
            onNone: e => "Error");

        Assert.Equal(TestUser.Name, result);
    }
}