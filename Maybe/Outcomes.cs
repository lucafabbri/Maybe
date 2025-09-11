namespace Maybe;
/// <summary>
/// Represents a successful, generic outcome of an operation.
/// </summary>
public readonly record struct Success : IOutcome
{
    /// <inheritdoc/>
    public OutcomeType Type => OutcomeType.Success;
}

/// <summary>
/// Represents the successful creation of a resource.
/// </summary>
public readonly record struct Created : IOutcome
{
    /// <inheritdoc/>
    public OutcomeType Type => OutcomeType.Created;
}

/// <summary>
/// Represents that a request has been accepted for processing.
/// </summary>
public readonly record struct Accepted : IOutcome
{
    /// <inheritdoc/>
    public OutcomeType Type => OutcomeType.Accepted;
}

/// <summary>
/// Represents the successful update of a resource.
/// </summary>
public readonly record struct Updated : IOutcome
{
    /// <inheritdoc/>
    public OutcomeType Type => OutcomeType.Updated;
}

/// <summary>
/// Represents an operation that resulted in no change to the resource's state.
/// </summary>
public readonly record struct Unchanged : IOutcome
{
    /// <inheritdoc/>
    public OutcomeType Type => OutcomeType.Unchanged;
}

/// <summary>
/// Represents the successful deletion of a resource.
/// </summary>
public readonly record struct Deleted : IOutcome
{
    /// <inheritdoc/>
    public OutcomeType Type => OutcomeType.Deleted;
}

/// <summary>
/// Represents a successful outcome where the data was retrieved from a cache.
/// This type wraps the cached value.
/// </summary>
/// <typeparam name="T">The type of the cached value.</typeparam>
public readonly record struct Cached<T>(T Value) : IOutcome
{
    /// <inheritdoc/>
    // Note: The outcome type is 'Success' because caching is a form of successful retrieval.
    // The information that it's cached is conveyed by the type `Cached<T>` itself.
    public OutcomeType Type => OutcomeType.Success;
}

/// <summary>
/// Provides singleton instances for common, stateless success outcomes for cleaner access.
/// </summary>
public static class Outcomes
{
    public static Success Success => default;
    public static Created Created => default;
    public static Accepted Accepted => default;
    public static Updated Updated => default;
    public static Unchanged Unchanged => default;
    public static Deleted Deleted => default;
}

/// <summary>
/// Represents a successful outcome of an update operation, providing a common contract
/// for results that can be either 'Updated' or 'Unchanged'.
/// </summary>
public interface IUpdateOutcome : IOutcome { }
