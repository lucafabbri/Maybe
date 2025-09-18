namespace Maybe;

/// <summary>
/// Represents an expected, plausible failure in a process that is not due to invalid input or a system exception.
/// </summary>
public class FailureError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FailureError"/> class with default values.
    /// </summary>
    public FailureError() : base(OutcomeType.Failure, "Default.Failure", "A failure has occurred.")
    {
        ContextData = [];
    }

    public FailureError(Error error) : this()
    {
        SetInnerError(error);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FailureError"/> class.
    /// </summary>
    /// <param name="message">A descriptive message for the failure.</param>
    /// <param name="code">A unique code identifying the failure.</param>
    /// <param name="contextData">A dictionary of contextual data for debugging and logging.</param>
    /// <param name="innerError">The error that is the cause of the current error, if any.</param>
    public FailureError(
        string message,
        string code,
        Dictionary<string, object>? contextData = null,
        BaseError? innerError = null)
        : base(OutcomeType.Failure, code, message, innerError)
    {
        ContextData = contextData ?? [];
    }

    /// <summary>
    /// Gets or sets additional contextual data about the failure for logging and debugging.
    /// This can include parameters, state, or other information relevant to the failed process.
    /// </summary>
    public Dictionary<string, object> ContextData { get; set; }
}

