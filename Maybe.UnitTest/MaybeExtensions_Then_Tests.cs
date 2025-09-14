using Xunit;
using static Maybe.Tests.TestData;

namespace Maybe.Tests;

public class MaybeExtensions_Then_Tests
{
    private Maybe<string, Error> SuccessFunc(TestValue value) => $"Processed: {value.Name}";
    private Maybe<string> ErrorFunc(TestValue value) => TestError;
    private Task<Maybe<string>> SuccessAsyncFunc(TestValue value) => Task.FromResult((Maybe<string>)$"Processed: {value.Name}");
    private Task<Maybe<string>> ErrorAsyncFunc(TestValue value) => Task.FromResult((Maybe<string>)TestError);

    // --- Then (Sync) ---

    [Fact]
    public void Then_WhenSuccess_AndFuncSucceeds_ReturnsNewSuccess()
    {
        // Arrange
        Maybe<TestValue> maybe = SuccessValue;

        // Act
        var result = maybe.Then(SuccessFunc);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Processed: Success", result.ValueOrThrow());
    }

    [Fact]
    public void Then_WhenSuccess_AndFuncFails_ReturnsNewError()
    {
        // Arrange
        Maybe<TestValue> maybe = SuccessValue;

        // Act
        var result = maybe.Then(ErrorFunc);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(TestError, result.ErrorOrThrow());
    }

    [Fact]
    public void Then_WhenError_DoesNotExecuteFuncAndPropagatesError()
    {
        // Arrange
        Maybe<TestValue> maybe = TestError;
        var funcWasCalled = false;
        Maybe<string> TrackableSuccessFunc(TestValue _)
        {
            funcWasCalled = true;
            return "Should not be called";
        }

        // Act
        var result = maybe.Then(TrackableSuccessFunc);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(TestError, result.ErrorOrThrow());
        Assert.False(funcWasCalled, "The chained function should not be executed on an error state.");
    }

    // --- ThenAsync (Sync source, Async func) ---

    [Fact]
    public async Task ThenAsync_WhenSuccess_ChainsAsynchronously()
    {
        // Arrange
        Maybe<TestValue> maybe = SuccessValue;

        // Act
        var result = await maybe.ThenAsync(SuccessAsyncFunc);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Processed: Success", result.ValueOrThrow());
    }

    [Fact]
    public async Task ThenAsync_WhenError_PropagatesErrorWithoutExecutingFunc()
    {
        // Arrange
        Maybe<TestValue> maybe = TestError;
        var funcWasCalled = false;
        Task<Maybe<string>> TrackableSuccessAsyncFunc(TestValue _)
        {
            funcWasCalled = true;
            return Task.FromResult((Maybe<string>)"Should not be called");
        }

        // Act
        var result = await maybe.ThenAsync(TrackableSuccessAsyncFunc);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(TestError, result.ErrorOrThrow());
        Assert.False(funcWasCalled, "The chained async function should not be executed on an error state.");
    }

    // --- Then (Async source, Sync func) ---

    [Fact]
    public async Task Then_OnTask_WhenSuccess_ChainsSynchronously()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<TestValue>)SuccessValue);

        // Act
        var result = await maybeTask.Then(SuccessFunc);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Processed: Success", result.ValueOrThrow());
    }

    [Fact]
    public async Task Then_OnTask_WhenError_PropagatesError()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<TestValue>)TestError);
        var funcWasCalled = false;
        Maybe<string> TrackableSuccessFunc(TestValue _)
        {
            funcWasCalled = true;
            return "Should not be called";
        }

        // Act
        var result = await maybeTask.Then(TrackableSuccessFunc);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(TestError, result.ErrorOrThrow());
        Assert.False(funcWasCalled, "The chained function should not be executed when the source task has an error.");
    }

    // --- ThenAsync (Async source, Async func) ---

    [Fact]
    public async Task ThenAsync_OnTask_WhenSuccess_ChainsAsynchronously()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<TestValue>)SuccessValue);

        // Act
        var result = await maybeTask.ThenAsync(SuccessAsyncFunc);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Processed: Success", result.ValueOrThrow());
    }

    [Fact]
    public async Task ThenAsync_OnTask_WhenError_PropagatesError()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<TestValue>)TestError);
        var funcWasCalled = false;
        Task<Maybe<string>> TrackableSuccessAsyncFunc(TestValue _)
        {
            funcWasCalled = true;
            return Task.FromResult((Maybe<string>)"Should not be called");
        }

        // Act
        var result = await maybeTask.ThenAsync(TrackableSuccessAsyncFunc);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(TestError, result.ErrorOrThrow());
        Assert.False(funcWasCalled, "The chained async function should not be executed when the source task has an error.");
    }
}


