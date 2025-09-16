using Maybe;

namespace Maybe.Toolkit;

/// <summary>
/// Error for HTTP request operations.
/// </summary>
public class HttpError : FailureError
{
    public override OutcomeType Type => OutcomeType.Failure;
    public override string Code => "Http.RequestError";
    public override string Message => "HTTP request failed.";

    /// <summary>
    /// The original exception that caused the HTTP error.
    /// </summary>
    public Exception? OriginalException { get; private set; }

    /// <summary>
    /// The URL that was being accessed when the error occurred.
    /// </summary>
    public string? RequestUrl { get; private set; }

    /// <summary>
    /// The HTTP status code if available.
    /// </summary>
    public System.Net.HttpStatusCode? StatusCode { get; private set; }

    public HttpError() { }

    public HttpError(Exception originalException, string? requestUrl = null, System.Net.HttpStatusCode? statusCode = null, string? customMessage = null)
    {
        OriginalException = originalException;
        RequestUrl = requestUrl;
        StatusCode = statusCode;
        if (customMessage != null)
        {
            Message = customMessage;
        }
    }
}