namespace Maybe;

/// <summary>
/// Represents an unexpected error, typically wrapping a system exception to provide more context while hiding sensitive details.
/// </summary>
public class UnexpectedError : Error
{
    private Exception _exception = new InvalidOperationException("Default exception for parameterless constructor.");

    /// <summary>
    /// Initializes a new instance of the <see cref="UnexpectedError"/> class with a default exception.
    /// </summary>
    public UnexpectedError() : base(OutcomeType.Unexpected, "System.Exception", "An unexpected error has occurred.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnexpectedError"/> class.
    /// </summary>
    /// <param name="exception">The original exception that caused this error.</param>
    /// <param name="message">An optional custom message. If null, the exception's message is used.</param>
    /// <param name="code">An optional custom code. If null, "System.Exception" is used.</param>
    public UnexpectedError(
        Exception exception,
        string? message = null,
        string? code = null)
        : base(OutcomeType.Unexpected, code ?? "System.Exception", message ?? exception.Message, null)
    {
        Exception = exception;
    }

    /// <summary>
    /// Gets or sets the original exception that caused this error.
    /// When set, it also populates the <see cref="BaseError.InnerError"/> with the exception's inner exception.
    /// This should be used for logging purposes and not exposed to the end-user.
    /// </summary>
    public Exception Exception
    {
        get => _exception;
        set
        {
            _exception = value;
            if (value?.InnerException is not null)
            {
                SetInnerError(new UnexpectedError(value.InnerException));
            }
        }
    }
}

