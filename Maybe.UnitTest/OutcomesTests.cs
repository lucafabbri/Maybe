using FluentAssertions;
using Maybe;
using Xunit;

namespace Maybe.Tests;

/// <summary>
/// Contains unit tests for the various Outcome structs and the Outcomes static class.
/// </summary>
public class OutcomesTests
{
    [Fact]
    public void Success_ShouldHaveCorrectOutcomeType()
    {
        // Arrange & Act
        var outcome = new Success();

        // Assert
        outcome.Type.Should().Be(OutcomeType.Success);
    }

    [Fact]
    public void Created_ShouldHaveCorrectOutcomeType()
    {
        // Arrange & Act
        var outcome = new Created();

        // Assert
        outcome.Type.Should().Be(OutcomeType.Created);
    }

    [Fact]
    public void Accepted_ShouldHaveCorrectOutcomeType()
    {
        // Arrange & Act
        var outcome = new Accepted();

        // Assert
        outcome.Type.Should().Be(OutcomeType.Accepted);
    }

    [Fact]
    public void Updated_ShouldHaveCorrectOutcomeType()
    {
        // Arrange & Act
        var outcome = new Updated();

        // Assert
        outcome.Type.Should().Be(OutcomeType.Updated);
    }

    [Fact]
    public void Unchanged_ShouldHaveCorrectOutcomeType()
    {
        // Arrange & Act
        var outcome = new Unchanged();

        // Assert
        outcome.Type.Should().Be(OutcomeType.Unchanged);
    }

    [Fact]
    public void Deleted_ShouldHaveCorrectOutcomeType()
    {
        // Arrange & Act
        var outcome = new Deleted();

        // Assert
        outcome.Type.Should().Be(OutcomeType.Deleted);
    }

    [Fact]
    public void Cached_ShouldStoreValueAndHaveSuccessOutcomeType()
    {
        // Arrange
        var cachedValue = "This is a cached string.";

        // Act
        var outcome = new Cached<string>(cachedValue);

        // Assert
        outcome.Type.Should().Be(OutcomeType.Success);
        outcome.Value.Should().Be(cachedValue);
    }

    [Fact]
    public void Cached_WithNullValue_ShouldStoreNull()
    {
        // Arrange & Act
        var outcome = new Cached<string?>(null);

        // Assert
        outcome.Type.Should().Be(OutcomeType.Success);
        outcome.Value.Should().BeNull();
    }

    [Fact]
    public void Outcomes_StaticProperties_ShouldReturnCorrectDefaultInstances()
    {
        // Assert
        Outcomes.Success.Should().Be(default(Success));
        Outcomes.Created.Should().Be(default(Created));
        Outcomes.Accepted.Should().Be(default(Accepted));
        Outcomes.Updated.Should().Be(default(Updated));
        Outcomes.Unchanged.Should().Be(default(Unchanged));
        Outcomes.Deleted.Should().Be(default(Deleted));
    }
}
