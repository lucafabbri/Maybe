using Maybe;

namespace Maybe.Toolkit;

/// <summary>
/// Error for collection access operations.
/// </summary>
public class CollectionError : FailureError
{
    public override OutcomeType Type => OutcomeType.Failure;
    public override string Code => "Collection.AccessError";
    public override string Message => "Collection access failed.";

    /// <summary>
    /// The key or index that was being accessed when the error occurred.
    /// </summary>
    public object? Key { get; private set; }

    /// <summary>
    /// The original exception that caused the collection error.
    /// </summary>
    public Exception? OriginalException { get; private set; }

    public CollectionError() { }

    public CollectionError(object key, Exception? originalException = null, string? customMessage = null)
    {
        Key = key;
        OriginalException = originalException;
        if (customMessage != null)
        {
            Message = customMessage;
        }
    }
}