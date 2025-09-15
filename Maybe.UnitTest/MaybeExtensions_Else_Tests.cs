using FluentAssertions;
using Maybe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains comprehensive unit tests for the 'Else' (Fallback) extension methods.
/// Tests cover edge cases, different error types, value types, null handling, chaining, and async patterns.
/// </summary>
public class MaybeExtensions_Else_Tests
{
    #region Test Data and Setup

    private record User(string Name);
    private record Product(int Id, string Name, decimal Price);
    
    // Different error types for comprehensive testing
    private class TestError : Error { }
    private class CustomValidationError : ValidationError 
    { 
        public CustomValidationError() : base(new Dictionary<string, string> { ["Name"] = "Invalid name" }) { }
    }
    private class CustomNotFoundError : NotFoundError 
    { 
        public CustomNotFoundError() : base("User", "123") { }
    }
    private class CustomAuthError : AuthorizationError 
    { 
        public CustomAuthError() : base(OutcomeType.Unauthorized, "ViewUser") { }
    }

    // Test data
    private static readonly User SuccessUser = new("Alice");
    private static readonly User FallbackUser = new("Guest");
    private static readonly Product SuccessProduct = new(1, "Laptop", 999.99m);
    private static readonly Product FallbackProduct = new(0, "Default", 0m);
    
    private static readonly TestError TestErrorInstance = new();
    private static readonly CustomValidationError ValidationErrorInstance = new();
    private static readonly CustomNotFoundError NotFoundErrorInstance = new();
    private static readonly CustomAuthError AuthErrorInstance = new();

    #endregion

    #region Synchronous Else Tests - Reference Types

    [Fact]
    public void Else_WithValue_OnSuccess_ReturnsOriginalValue()
    {
        // Arrange
        Maybe<User, TestError> maybe = SuccessUser;

        // Act
        var result = maybe.Else(FallbackUser);

        // Assert
        result.Should().Be(SuccessUser);
        result.Should().BeSameAs(SuccessUser);
    }

    [Fact]
    public void Else_WithValue_OnError_ReturnsFallbackValue()
    {
        // Arrange
        Maybe<User, TestError> maybe = TestErrorInstance;

        // Act
        var result = maybe.Else(FallbackUser);

        // Assert
        result.Should().Be(FallbackUser);
        result.Should().BeSameAs(FallbackUser);
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
        result.Should().Be(SuccessUser);
        wasCalled.Should().BeFalse();
    }

    [Fact]
    public void Else_WithFunc_OnError_InvokesFuncAndReturnsItsValue()
    {
        // Arrange
        Maybe<User, TestError> maybe = TestErrorInstance;
        var wasCalled = false;
        Func<TestError, User> fallbackFunc = e => { wasCalled = true; return FallbackUser; };

        // Act
        var result = maybe.Else(fallbackFunc);

        // Assert
        result.Should().Be(FallbackUser);
        wasCalled.Should().BeTrue();
    }

    [Fact]
    public void Else_WithFunc_OnError_PassesCorrectErrorToFunction()
    {
        // Arrange
        Maybe<User, TestError> maybe = TestErrorInstance;
        TestError? receivedError = null;
        Func<TestError, User> fallbackFunc = e => { receivedError = e; return FallbackUser; };

        // Act
        var result = maybe.Else(fallbackFunc);

        // Assert
        receivedError.Should().BeSameAs(TestErrorInstance);
        result.Should().Be(FallbackUser);
    }

    #endregion

    #region Synchronous Else Tests - Value Types

    [Theory]
    [InlineData(42, 0, 42)]
    [InlineData(42, 99, 42)]
    public void Else_WithValueType_OnSuccess_ReturnsOriginalValue(int originalValue, int fallbackValue, int expectedResult)
    {
        // Arrange
        Maybe<int, TestError> maybe = originalValue;

        // Act
        var result = maybe.Else(fallbackValue);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(99, 99)]
    [InlineData(-1, -1)]
    public void Else_WithValueType_OnError_ReturnsFallbackValue(int fallbackValue, int expectedResult)
    {
        // Arrange
        Maybe<int, TestError> maybe = TestErrorInstance;

        // Act
        var result = maybe.Else(fallbackValue);

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void Else_WithBool_OnSuccess_ReturnsOriginalValue()
    {
        // Arrange
        Maybe<bool, TestError> maybe = true;

        // Act
        var result = maybe.Else(false);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Else_WithBool_OnError_ReturnsFallbackValue()
    {
        // Arrange
        Maybe<bool, TestError> maybe = TestErrorInstance;

        // Act
        var result = maybe.Else(false);

        // Assert
        result.Should().BeFalse();
    }

    #endregion

    #region Null and Default Value Tests

    [Fact]
    public void Else_WithNullFallbackValue_OnError_ReturnsNull()
    {
        // Arrange
        Maybe<User?, TestError> maybe = TestErrorInstance;

        // Act
        var result = maybe.Else((User?)null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Else_WithNullReturningFunc_OnError_ReturnsNull()
    {
        // Arrange
        Maybe<User?, TestError> maybe = TestErrorInstance;
        Func<TestError, User?> fallbackFunc = e => null;

        // Act
        var result = maybe.Else(fallbackFunc);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Else_WithStringFallback_HandlesEmptyAndNull()
    {
        // Arrange
        Maybe<string?, TestError> maybeError = TestErrorInstance;

        // Act & Assert
        maybeError.Else("").Should().Be("");
        maybeError.Else((string?)null).Should().BeNull();
        maybeError.Else("fallback").Should().Be("fallback");
    }

    #endregion

    #region Different Error Types Tests

    [Fact]
    public void Else_WithValidationError_ReturnsCorrectFallback()
    {
        // Arrange
        Maybe<User, CustomValidationError> maybe = ValidationErrorInstance;

        // Act
        var result = maybe.Else(FallbackUser);

        // Assert
        result.Should().Be(FallbackUser);
    }

    [Fact]
    public void Else_WithNotFoundError_ReturnsCorrectFallback()
    {
        // Arrange
        Maybe<Product, CustomNotFoundError> maybe = NotFoundErrorInstance;

        // Act
        var result = maybe.Else(FallbackProduct);

        // Assert
        result.Should().Be(FallbackProduct);
    }

    [Fact]
    public void Else_WithAuthorizationError_ReturnsCorrectFallback()
    {
        // Arrange
        Maybe<string, CustomAuthError> maybe = AuthErrorInstance;

        // Act
        var result = maybe.Else("unauthorized_fallback");

        // Assert
        result.Should().Be("unauthorized_fallback");
    }

    [Fact]
    public void Else_WithFunc_DifferentErrorTypes_PassesCorrectErrorInstance()
    {
        // Arrange & Act & Assert
        var validationMaybe = (Maybe<User, CustomValidationError>)ValidationErrorInstance;
        CustomValidationError? receivedValidationError = null;
        validationMaybe.Else(e => { receivedValidationError = e; return FallbackUser; });
        receivedValidationError.Should().BeSameAs(ValidationErrorInstance);

        var notFoundMaybe = (Maybe<Product, CustomNotFoundError>)NotFoundErrorInstance;
        CustomNotFoundError? receivedNotFoundError = null;
        notFoundMaybe.Else(e => { receivedNotFoundError = e; return FallbackProduct; });
        receivedNotFoundError.Should().BeSameAs(NotFoundErrorInstance);
    }

    #endregion

    #region Exception Handling in Fallback Functions

    [Fact]
    public void Else_WithFuncThatThrows_PropagatesException()
    {
        // Arrange
        Maybe<User, TestError> maybe = TestErrorInstance;
        Func<TestError, User> fallbackFunc = e => throw new InvalidOperationException("Fallback failed");

        // Act & Assert
        Action act = () => maybe.Else(fallbackFunc);
        act.Should().Throw<InvalidOperationException>().WithMessage("Fallback failed");
    }

    #endregion

    #region Asynchronous Else Tests

    [Fact]
    public async Task Else_OnSuccessTask_ReturnsOriginalValue()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<User, TestError>)SuccessUser);

        // Act
        var resultWithValue = await maybeTask.Else(FallbackUser);
        var resultWithFunc = await maybeTask.Else(e => FallbackUser);

        // Assert
        resultWithValue.Should().Be(SuccessUser);
        resultWithFunc.Should().Be(SuccessUser);
    }

    [Fact]
    public async Task Else_OnErrorTask_ReturnsFallbackValue()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<User, TestError>)TestErrorInstance);

        // Act
        var resultWithValue = await maybeTask.Else(FallbackUser);
        var resultWithFunc = await maybeTask.Else(e => FallbackUser);

        // Assert
        resultWithValue.Should().Be(FallbackUser);
        resultWithFunc.Should().Be(FallbackUser);
    }

    [Fact]
    public async Task Else_OnTaskWithValueType_WorksCorrectly()
    {
        // Arrange
        var successTask = Task.FromResult((Maybe<int, TestError>)42);
        var errorTask = Task.FromResult((Maybe<int, TestError>)TestErrorInstance);

        // Act & Assert
        (await successTask.Else(0)).Should().Be(42);
        (await errorTask.Else(99)).Should().Be(99);
        (await successTask.Else(e => 0)).Should().Be(42);
        (await errorTask.Else(e => 99)).Should().Be(99);
    }

    #endregion

    #region ElseAsync Tests

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
        result.Should().Be(SuccessUser);
        wasCalled.Should().BeFalse();
    }

    [Fact]
    public async Task ElseAsync_OnError_InvokesFuncAndReturnsItsValue()
    {
        // Arrange
        Maybe<User, TestError> maybe = TestErrorInstance;
        var wasCalled = false;
        Func<TestError, Task<User>> fallbackFunc = e => { wasCalled = true; return Task.FromResult(FallbackUser); };

        // Act
        var result = await maybe.ElseAsync(fallbackFunc);

        // Assert
        result.Should().Be(FallbackUser);
        wasCalled.Should().BeTrue();
    }

    [Fact]
    public async Task ElseAsync_WithDelayedTask_WorksCorrectly()
    {
        // Arrange
        Maybe<string, TestError> maybe = TestErrorInstance;
        Func<TestError, Task<string>> fallbackFunc = async e =>
        {
            await Task.Delay(10); // Simulate async work
            return "delayed_result";
        };

        // Act
        var result = await maybe.ElseAsync(fallbackFunc);

        // Assert
        result.Should().Be("delayed_result");
    }

    [Fact]
    public async Task ElseAsync_WithExceptionInAsyncFunc_PropagatesException()
    {
        // Arrange
        Maybe<User, TestError> maybe = TestErrorInstance;
        Func<TestError, Task<User>> fallbackFunc = e => Task.FromException<User>(new InvalidOperationException("Async fallback failed"));

        // Act & Assert
        await FluentActions.Awaiting(() => maybe.ElseAsync(fallbackFunc))
            .Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Async fallback failed");
    }

    #endregion

    #region ElseAsync on Task<Maybe> Tests

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
        result.Should().Be(SuccessUser);
        wasCalled.Should().BeFalse();
    }

    [Fact]
    public async Task ElseAsync_OnErrorTask_ReturnsFallbackValue()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<User, TestError>)TestErrorInstance);
        var wasCalled = false;
        Func<TestError, Task<User>> fallbackFunc = e => { wasCalled = true; return Task.FromResult(FallbackUser); };

        // Act
        var result = await maybeTask.ElseAsync(fallbackFunc);

        // Assert
        result.Should().Be(FallbackUser);
        wasCalled.Should().BeTrue();
    }

    [Fact]
    public async Task ElseAsync_OnTaskWithValueType_WorksCorrectly()
    {
        // Arrange
        var successTask = Task.FromResult((Maybe<int, TestError>)42);
        var errorTask = Task.FromResult((Maybe<int, TestError>)TestErrorInstance);
        
        Func<TestError, Task<int>> fallbackFunc = e => Task.FromResult(99);

        // Act & Assert
        (await successTask.ElseAsync(fallbackFunc)).Should().Be(42);
        (await errorTask.ElseAsync(fallbackFunc)).Should().Be(99);
    }

    #endregion

    #region Chaining and Complex Scenarios

    [Fact]
    public void Else_Chaining_MultipleElseCalls()
    {
        // Arrange
        Maybe<string, TestError> maybe = TestErrorInstance;

        // Act
        var result = maybe
            .Else("first_fallback")
            .ToUpper(); // Chaining with string method

        // Assert
        result.Should().Be("FIRST_FALLBACK");
    }

    [Fact]
    public void Else_AfterSelect_WorksCorrectly()
    {
        // Arrange
        Maybe<User, TestError> successMaybe = SuccessUser;
        Maybe<User, TestError> errorMaybe = TestErrorInstance;

        // Act & Assert
        successMaybe.Select(u => u.Name).Else("default").Should().Be("Alice");
        errorMaybe.Select(u => u.Name).Else("default").Should().Be("default");
    }

    [Fact]
    public async Task Else_WithAsyncChaining_WorksCorrectly()
    {
        // Arrange
        var maybeTask = Task.FromResult((Maybe<User, TestError>)TestErrorInstance);
        
        // Act
        var result = await maybeTask
            .Select(u => u.Name.ToUpper())
            .Else("DEFAULT_USER");

        // Assert
        result.Should().Be("DEFAULT_USER");
    }

    #endregion

    #region Performance and Memory Tests

    [Fact]
    public void Else_WithLargeObject_DoesNotCausePerformanceIssues()
    {
        // Arrange
        var largeArray = new int[10000];
        for (int i = 0; i < largeArray.Length; i++) largeArray[i] = i;
        
        var fallbackArray = new int[10000];
        Maybe<int[], TestError> maybe = TestErrorInstance;

        // Act & Assert
        var result = maybe.Else(fallbackArray);
        result.Should().BeSameAs(fallbackArray);
    }

    [Fact]
    public void Else_DoesNotEvaluateExpensiveFallbackOnSuccess()
    {
        // Arrange
        Maybe<int, TestError> maybe = 42;
        var evaluationCount = 0;
        
        // Create a function that tracks if it's called
        Func<TestError, int> fallbackFunc = e => { evaluationCount++; return 999; };
        
        // Act
        var result = maybe.Else(fallbackFunc);

        // Assert - fallback function should not be called for success case
        result.Should().Be(42);
        evaluationCount.Should().Be(0);
    }

    #endregion
}
