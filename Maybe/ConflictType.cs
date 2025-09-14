namespace Maybe;

/// <summary>
/// Defines the nature of a conflict error.
/// </summary>
public enum ConflictType
{
    /// <summary>
    /// A resource with the same unique identifier already exists (e.g., duplicate email or username).
    /// </summary>
    Duplicate,

    /// <summary>
    /// The resource has been modified by another process since it was last read (optimistic concurrency failure).
    /// </summary>
    StaleState,

    /// <summary>
    /// The requested operation violates a business rule related to the resource's current state (e.g., trying to cancel an order that has already been shipped).
    /// </summary>
    BusinessRuleViolation
}

