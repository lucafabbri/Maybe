using FluentAssertions;
using Maybe;
using System.Threading.Tasks;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains unit tests for the 'Then' (Bind/FlatMap) extension methods.
/// </summary>
public class MaybeExtensions_Then_Tests
{
    // --- Test Data ---
    private record TestValue(string Name);
    private class FirstError : Error { }
    private class SecondError : Error { }

    private static readonly TestValue SuccessValue = new("Success");
    private static readonly FirstError TestError1 = new();
    private static readonly SecondError TestError2 = new();

    // --- Test Functions ---
    private Maybe<string, SecondError> SuccessFunc(TestValue value) => $"Processed: {value.Name}";
    private Maybe<string, SecondError> ErrorFunc(TestValue value) => TestError2;
    private Task<Maybe<string, SecondError>> SuccessAsyncFunc(TestValue value) => Task.FromResult((Maybe<string, SecondError>)$"Processed: {value.Name}");
    private Task<Maybe<string, SecondError>> ErrorAsyncFunc(TestValue value) => Task.FromResult((Maybe<string, SecondError>)TestError2);


    // --- Then (Sync -> Sync) ---

    [Fact]
    public void Then_OnSuccess_WhenFuncSucceeds_ReturnsNewSuccess()
    {
        // Arrange
        Maybe<TestValue, FirstError> maybe = SuccessValue;

        // Act
        var result = maybe.Then(SuccessFunc);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be("Processed: Success");
    }

    [Fact]
    public void Then_OnSuccess_WhenFuncFails_ReturnsNewError()
    {
        // Arrange
        Maybe<TestValue, FirstError> maybe = SuccessValue;

        // Act
        var result = maybe.Then(ErrorFunc);

        // Assert
        result.IsError.Should().BeTrue();
        result.ErrorOrThrow().Should().Be(TestError2);
    }

    [Fact]
    public void Then_OnError_WhenErrorTypesAreCompatible_PropagatesCastedError()
    {
        // Arrange
        var specificError = new NotFoundError();
        Maybe<TestValue, NotFoundError> maybe = specificError;

        // The function expects a more generic Error, which NotFoundError is.
        Maybe<string, Error> Func(TestValue _) => "Should not be called";

        // Act
        var result = maybe.Then(Func);

        // Assert
        result.IsError.Should().BeTrue();
        result.ErrorOrThrow().Should().BeSameAs(specificError);
    }

    [Fact]
    public void Then_OnError_WhenErrorTypesAreIncompatible_CreatesNewErrorWithInnerError()
    {
        // Arrange
        Maybe<TestValue, FirstError> maybe = TestError1;
        var funcWasCalled = false;
        Maybe<string, SecondError> TrackableFunc(TestValue _) { funcWasCalled = true; return "X"; }

        // Act
        var result = maybe.Then(TrackableFunc);

        // Assert
        result.IsError.Should().BeTrue();
        funcWasCalled.Should().BeFalse();
        var error = result.ErrorOrThrow().Should().BeOfType<SecondError>().Subject;
        error.InnerError.Should().BeSameAs(TestError1);
    }

    // --- ThenAsync (Sync -> Async) ---

    [Fact]
    public async Task ThenAsync_OnSuccess_WhenFuncSucceeds_ReturnsNewSuccess()
    {
        // Arrange
        Maybe<TestValue, FirstError> maybe = SuccessValue;

        // Act
        var result = await maybe.ThenAsync(SuccessAsyncFunc);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be("Processed: Success");
    }

    [Fact]
    public async Task ThenAsync_OnSuccess_WhenFuncFails_ReturnsNewError()
    {
        // Arrange
        Maybe<TestValue, FirstError> maybe = SuccessValue;

        // Act
        var result = await maybe.ThenAsync(ErrorAsyncFunc);

        // Assert
        result.IsError.Should().BeTrue();
        result.ErrorOrThrow().Should().Be(TestError2);
    }

    [Fact]
    public async Task ThenAsync_OnError_WhenErrorTypesAreIncompatible_CreatesNewError()
    {
        // Arrange
        Maybe<TestValue, FirstError> maybe = TestError1;
        var funcWasCalled = false;
        Task<Maybe<string, SecondError>> TrackableFunc(TestValue _) { funcWasCalled = true; return Task.FromResult((Maybe<string, SecondError>)"X"); }

        // Act
        var result = await maybe.ThenAsync(TrackableFunc);

        // Assert
        result.IsError.Should().BeTrue();
        funcWasCalled.Should().BeFalse();
        var error = result.ErrorOrThrow().Should().BeOfType<SecondError>().Subject;
        error.InnerError.Should().BeSameAs(TestError1);
    }

    // --- Then (Async -> Sync) ---

    [Fact]
    public async Task Then_OnSuccessTask_WhenFuncSucceeds_ReturnsNewSuccess()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<TestValue, FirstError>)SuccessValue);

        // Act
        var result = await maybeTask.Then(SuccessFunc);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be("Processed: Success");
    }

    [Fact]
    public async Task Then_OnSuccessTask_WhenFuncFails_ReturnsNewError()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<TestValue, FirstError>)SuccessValue);

        // Act
        var result = await maybeTask.Then(ErrorFunc);

        // Assert
        result.IsError.Should().BeTrue();
        result.ErrorOrThrow().Should().Be(TestError2);
    }

    [Fact]
    public async Task Then_OnErrorTask_PropagatesIncompatibleError()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<TestValue, FirstError>)TestError1);
        var funcWasCalled = false;
        Maybe<string, SecondError> TrackableFunc(TestValue _) { funcWasCalled = true; return "X"; }

        // Act
        var result = await maybeTask.Then(TrackableFunc);

        // Assert
        result.IsError.Should().BeTrue();
        funcWasCalled.Should().BeFalse();
        result.ErrorOrThrow().InnerError.Should().BeSameAs(TestError1);
    }

    // --- ThenAsync (Async -> Async) ---

    [Fact]
    public async Task ThenAsync_OnSuccessTask_WhenFuncSucceeds_ReturnsNewSuccess()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<TestValue, FirstError>)SuccessValue);

        // Act
        var result = await maybeTask.ThenAsync(SuccessAsyncFunc);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ValueOrThrow().Should().Be("Processed: Success");
    }

    [Fact]
    public async Task ThenAsync_OnSuccessTask_WhenFuncFails_ReturnsNewError()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<TestValue, FirstError>)SuccessValue);

        // Act
        var result = await maybeTask.ThenAsync(ErrorAsyncFunc);

        // Assert
        result.IsError.Should().BeTrue();
        result.ErrorOrThrow().Should().Be(TestError2);
    }

    [Fact]
    public async Task ThenAsync_OnErrorTask_PropagatesIncompatibleError()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<TestValue, FirstError>)TestError1);
        var funcWasCalled = false;
        Task<Maybe<string, SecondError>> TrackableFunc(TestValue _) { funcWasCalled = true; return Task.FromResult((Maybe<string, SecondError>)"X"); }

        // Act
        var result = await maybeTask.ThenAsync(TrackableFunc);

        // Assert
        result.IsError.Should().BeTrue();
        funcWasCalled.Should().BeFalse();
        result.ErrorOrThrow().InnerError.Should().BeSameAs(TestError1);
    }
}
