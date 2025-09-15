namespace Maybe.Tests;

/// <summary>
/// Contains tests specifically for verifying the behavior when custom user-defined types
/// implement the IOutcome interface.
/// </summary>
public class MaybeIOutcomeTests
{
    /// <summary>
    /// A custom, user-defined success type that implements IOutcome to provide a specific OutcomeType.
    /// </summary>
    private record CustomAcceptedOutcome(string RequestId) : IOutcome
    {
        // This custom type reports its state as 'Accepted', not the default 'Success'.
        public OutcomeType Type => OutcomeType.Accepted;
    }

    [Fact]
    public void Type_WhenValueIsCustomIOutcome_ReturnsCustomOutcomeType()
    {
        // Arrange
        var customOutcome = new CustomAcceptedOutcome("Request-123");
        Maybe<CustomAcceptedOutcome, TestCustomError> maybe = customOutcome;

        // Act
        var outcomeType = maybe.Type;

        // Assert
        // The test verifies that the `Type` property correctly inspects the value
        // and returns the type defined in the custom class, not the default `OutcomeType.Success`.
        Assert.Equal(OutcomeType.Accepted, outcomeType);
    }

    [Fact]
    public void Match_WhenValueIsCustomIOutcome_ExecutesOnSomePathCorrectly()
    {
        // Arrange
        var customOutcome = new CustomAcceptedOutcome("Request-123");
        Maybe<CustomAcceptedOutcome, TestCustomError> maybe = customOutcome;

        // Act
        var result = maybe.Match(
            onSome: value =>
            {
                // We should receive the full, typed custom object here.
                Assert.IsType<CustomAcceptedOutcome>(value);
                return $"Success: {value.RequestId}";
            },
            onNone: error => "Should not be called");

        // Assert
        Assert.Equal("Success: Request-123", result);
    }

    [Fact]
    public async Task Then_WhenValueIsCustomIOutcome_ChainsCorrectly()
    {
        // Arrange
        var customOutcome = new CustomAcceptedOutcome("Request-123");
        var maybeTask = Task.FromResult((Maybe<CustomAcceptedOutcome, TestCustomError>)customOutcome);

        // Act
        var result = await maybeTask
            .Select(outcome => outcome.RequestId)
            .Then(requestId => $"Processed: {requestId}");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Processed: Request-123", result.ValueOrThrow());
    }
}


