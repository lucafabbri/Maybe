namespace Maybe;
/// <summary>
/// Represents a contract for a standardized error, providing a consistent
/// structure for handling failures across the application.
/// </summary>
public interface IError
{
    /// <summary>
    /// Gets the type of the outcome, categorizing the error.
    /// </summary>
    OutcomeType Type { get; }

    /// <summary>
    /// Gets a unique, machine-readable code for the error (e.g., "Users.NotFound").
    /// </summary>
    string Code { get; }

    /// <summary>
    /// Gets a human-readable message describing the error.
    /// </summary>
    string Message { get; }
}
