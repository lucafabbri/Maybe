namespace Maybe;

/// <summary>
/// Represents a conflict with the current state of a resource, such as a duplicate entity or a stale state for optimistic concurrency.
/// </summary>
public class ConflictError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictError"/> class with default values.
    /// </summary>
    public ConflictError() : base(OutcomeType.Conflict, "Default.Conflict", "A conflict error has occurred.")
    {
        ConflictType = ConflictType.BusinessRuleViolation;
        ResourceType = string.Empty;
        ConflictingParameters = new Dictionary<string, object>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictError"/> class.
    /// </summary>
    /// <param name="type">The specific type of conflict.</param>
    /// <param name="resourceType">The type of the resource that has the conflict (e.g., "User").</param>
    /// <param name="conflictingParameters">A dictionary of parameters that caused the conflict.</param>
    /// <param name="message">An optional custom message. A default message is generated if not provided.</param>
    /// <param name="code">An optional custom code. A default code is generated if not provided.</param>
    /// <param name="innerError">The error that is the cause of the current error, if any.</param>
    public ConflictError(
        ConflictType type,
        string resourceType,
        IReadOnlyDictionary<string, object> conflictingParameters,
        string? message = null,
        string? code = null,
        BaseError? innerError = null)
        : base(OutcomeType.Conflict, code ?? $"Conflict.{type}", message ?? $"A {type} conflict occurred on resource '{resourceType}'.", innerError)
    {
        ConflictType = type;
        ResourceType = resourceType;
        ConflictingParameters = conflictingParameters;
    }

    /// <summary>
    /// Gets or sets the specific type of conflict.
    /// </summary>
    public ConflictType ConflictType { get; set; }

    /// <summary>
    /// Gets or sets the type of the resource that has the conflict (e.g., "Order", "Product").
    /// </summary>
    public string ResourceType { get; set; }

    /// <summary>
    /// Gets or sets a dictionary of parameters and values that caused the conflict.
    /// </summary>
    public IReadOnlyDictionary<string, object> ConflictingParameters { get; set; }
}

