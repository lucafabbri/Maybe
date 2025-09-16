using Maybe;

namespace Maybe.Toolkit;

/// <summary>
/// Represents an error that occurred during combined HTTP and JSON operations.
/// This error can wrap either an HttpError or a JsonError.
/// </summary>
public class HttpJsonError : FailureError
{
    private readonly BaseError? _innerError;

    /// <summary>
    /// Initializes a new instance of the HttpJsonError class.
    /// Required for Maybe constraint.
    /// </summary>
    public HttpJsonError() : base("Unknown HTTP/JSON error", "HTTP_JSON_UNKNOWN")
    {
    }

    /// <summary>
    /// Initializes a new instance of the HttpJsonError class with an HttpError.
    /// </summary>
    /// <param name="httpError">The HTTP error that occurred.</param>
    public HttpJsonError(HttpError httpError) : base(httpError.Message, $"HTTP_JSON_{httpError.Code}")
    {
        _innerError = httpError;
    }

    /// <summary>
    /// Initializes a new instance of the HttpJsonError class with a JsonError.
    /// </summary>
    /// <param name="jsonError">The JSON error that occurred.</param>
    public HttpJsonError(JsonError jsonError) : base(jsonError.Message, $"HTTP_JSON_{jsonError.Code}")
    {
        _innerError = jsonError;
    }

    /// <summary>
    /// Gets the original HTTP error if this error was caused by an HTTP operation.
    /// </summary>
    public HttpError? HttpError => _innerError as HttpError;

    /// <summary>
    /// Gets the original JSON error if this error was caused by a JSON operation.
    /// </summary>
    public JsonError? JsonError => _innerError as JsonError;

    /// <summary>
    /// Gets the underlying error that caused this composite error.
    /// </summary>
    public BaseError? UnderlyingError => _innerError;

    /// <summary>
    /// Returns true if this error was caused by an HTTP operation.
    /// </summary>
    public bool IsHttpError => _innerError is HttpError;

    /// <summary>
    /// Returns true if this error was caused by a JSON operation.
    /// </summary>
    public bool IsJsonError => _innerError is JsonError;
}