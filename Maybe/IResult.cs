namespace Maybe;
/// <summary>
/// Represents a contract for any operation outcome, providing a standardized
/// way to retrieve its classification type.
/// </summary>
public interface IOutcome
{
    /// <summary>
    /// Gets the type of the outcome, categorizing the result.
    /// </summary>
    OutcomeType Type { get; }
}
