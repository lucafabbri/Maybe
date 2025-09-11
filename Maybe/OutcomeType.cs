namespace Maybe;
/// <summary>
/// Defines the standardized outcome states for an operation,
/// categorizing both success and failure scenarios.
/// </summary>
public enum OutcomeType
{
    // --- Success States ---

    /// <summary>
    /// Represents a generic, successful outcome.
    /// </summary>
    Success,

    /// <summary>
    /// Represents the successful creation of a new resource.
    /// </summary>
    Created,

    /// <summary>
    /// Represents that a request has been accepted for processing, but the processing has not been completed.
    /// Ideal for long-running or asynchronous operations.
    /// </summary>
    Accepted,

    /// <summary>
    /// Represents the successful update of a resource.
    /// </summary>
    Updated,

    /// <summary>
    /// Represents the successful completion of an operation that resulted in no change to the resource's state.
    /// </summary>
    Unchanged,

    /// <summary>
    /// Represents the successful deletion of a resource.
    /// </summary>
    Deleted,

    // --- Failure States ---

    /// <summary>
    /// The request could not be understood or processed due to invalid syntax or data.
    /// </summary>
    Validation,

    /// <summary>
    /// The request requires user authentication.
    /// </summary>
    Unauthorized,

    /// <summary>
    /// The server understood the request, but refuses to authorize it.
    /// </summary>
    Forbidden,

    /// <summary>
    /// The requested resource could not be found.
    /// </summary>
    NotFound,

    /// <summary>
    /// The request could not be completed due to a conflict with the current state of the resource.
    /// </summary>
    Conflict,

    /// <summary>
    /// The resource that is being accessed is locked.
    /// </summary>
    Locked,

    /// <summary>
    /// The user has sent too many requests in a given amount of time.
    /// </summary>
    Throttled,

    /// <summary>
    /// Represents a generic, predictable failure in the application logic.
    /// </summary>
    Failure,

    /// <summary>
    /// An unexpected, unhandled error occurred.
    /// </summary>
    Unexpected,
}
