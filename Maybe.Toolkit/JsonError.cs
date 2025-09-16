using Maybe;

namespace Maybe.Toolkit;

/// <summary>
/// Error for JSON serialization/deserialization failures.
/// </summary>
public class JsonError : FailureError
{
    public override OutcomeType Type => OutcomeType.Failure;
    public override string Code => "Json.SerializationError";
    public override string Message => "JSON operation failed.";

    /// <summary>
    /// The original exception that caused the JSON error.
    /// </summary>
    public Exception? OriginalException { get; private set; }

    public JsonError() { }

    public JsonError(Exception originalException, string? customMessage = null)
    {
        OriginalException = originalException;
        if (customMessage != null)
        {
            Message = customMessage;
        }
    }
}