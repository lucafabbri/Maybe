using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Maybe;

/// <summary>
/// Provides a default, concrete implementation of IError for common scenarios.
/// </summary>
public abstract class Error : IError
{
    Error? _innerError;

    /// <summary>
    /// Gets the inner error, if any, that caused this error.
    /// </summary>
    public Error? InnerError => _innerError;

    /// <inheritdoc/>
    public abstract OutcomeType Type { get; }

    /// <inheritdoc/>
    public abstract string Code { get; }

    /// <inheritdoc/>
    public abstract string Message { get; }

    /// <inheritdoc/>
    public int TimeStamp { get; } = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    public Error()
    {
    }

    internal void SetInnerError(Error innerError)
    {
        _innerError = innerError;
    }

    // Static factory methods for creating default errors with clear intent.

    public static Error Custom(OutcomeType type, string code, string message, Error? innerError = null) =>
        new ErrorImpl(type, code, message, innerError);

    public static Error Failure(
        string message = "A failure has occurred.",
        string code = "Default.Failure",
        Dictionary<string, object>? contextData = null,
        Error? innerError = null)
        => new FailureError(message, code, contextData, innerError);

    public static Error Unexpected(Exception exception, string? message = null, string? code = null) =>
        new UnexpectedError(exception, message, code);

    public static Error Validation(Dictionary<string, string> fieldsErrors, string message = "A validation error has occurred.", string code = "Default.Validation", Error? innerError = null) =>
        new ValidationError(fieldsErrors, message, code, innerError);

    public static Error Conflict(
    ConflictType type,
    string resourceType,
    Dictionary<string, object> conflictingParameters,
    string? message = null,
    string? code = null,
    Error? innerError = null) => new ConflictError(type, resourceType, conflictingParameters, message, code, innerError);

    public static Error NotFound(string itemName, object identifier, string? message = null, string? code = null, Error? innerError = null) =>
        new NotFoundError(itemName, identifier, message, code, innerError);

    public static Error Unauthorized(
        string action,
        string? resourceIdentifier = null,
        string? userId = null,
        string? message = null,
        string? code = null,
        Error? innerError = null)
        => new AuthorizationError(OutcomeType.Unauthorized, action, resourceIdentifier, userId, message, code, innerError);

    public static Error Forbidden(
        string action,
        string? resourceIdentifier = null,
        string? userId = null,
        string? message = null,
        string? code = null,
        Error? innerError = null)
        => new AuthorizationError(OutcomeType.Forbidden, action, resourceIdentifier, userId, message, code, innerError);


    public override bool Equals(object? obj)
    {
        return obj?.GetType() == GetType() && obj is Error other && Equals(other);
    }

    public bool Equals(IError other)
    {
        if (other is null) return false;
        return Type == other.Type && Code == other.Code;
    }

    /// <summary>
    /// Returns a simple string representation of the error.
    /// </summary>
    /// <returns>A string in the format "[Type] Code: Message".</returns>
    public override string ToString()
    {
        return $"[{Type}] {Code}: {Message}";
    }

    /// <summary>
    /// Returns a full, formatted string representation of the entire error chain.
    /// The layout uses aligned columns and wraps long messages for readability.
    /// </summary>
    /// <returns>The formatted string of the entire error chain.</returns>
    public string ToFullString()
    {
        // 1. Collect all errors and their hierarchy depth
        var allErrors = new List<(int depth, Error error)>();
        CollectErrors(this, 0, allErrors);
        if (!allErrors.Any()) return string.Empty;

        // 2. Define layout constants
        const int totalWidth = 120;
        const int timestampColumnWidth = 24; // Width for "[YYYY-MM-DD HH:MM:SS]  "

        // 3. Calculate dynamic column widths based on content
        int maxDepth = allErrors.Max(e => e.depth);
        int maxTypeWidth = allErrors.Max(e => $"[{e.error.Type}]".Length) + 1 + (maxDepth * 2); // +1 for space
        int maxCodeWidth = allErrors.Max(e => e.error.Code.Length) + 3; // +3 for spaces around

        var sb = new StringBuilder();

        foreach (var (depth, error) in allErrors)
        {
            // 4. Prepare parts for the current error line
            string indent = new string(' ', depth * 2);
            string typePart = $"{indent}[{error.Type}]".PadRight(maxTypeWidth);
            string codePart = error.Code.PadRight(maxCodeWidth);
            string timestampPart = $"[{DateTimeOffset.FromUnixTimeSeconds(error.TimeStamp).ToLocalTime():yyyy-MM-dd HH:mm:ss}]".PadRight(timestampColumnWidth);

            // 5. Calculate available width for the message and wrap it
            int messageColumnStart = maxTypeWidth + maxCodeWidth + timestampColumnWidth;
            int messageMaxWidth = totalWidth - messageColumnStart;
            var messageLines = WrapText(error.Message, messageMaxWidth);

            // 6. Append the first line containing all columns
            string firstLine = messageLines.FirstOrDefault() ?? string.Empty;
            sb.AppendLine($"{typePart}{codePart}{timestampPart}{firstLine}");

            // 7. Append subsequent lines from the wrapped message, indented correctly
            string messageIndent = new string(' ', messageColumnStart);
            foreach (var line in messageLines.Skip(1))
            {
                sb.AppendLine($"{messageIndent}{line}");
            }
        }

        return sb.ToString().TrimEnd();
    }

    /// <summary>
    /// Recursively collects all inner errors into a list with their depth.
    /// </summary>
    private void CollectErrors(Error? current, int depth, List<(int depth, Error error)> list)
    {
        if (current == null) return;
        list.Add((depth, current));
        CollectErrors(current.InnerError, depth + 1, list);
    }

    /// <summary>
    /// Wraps a string into multiple lines based on a maximum width.
    /// </summary>
    private List<string> WrapText(string text, int maxWidth)
    {
        if (string.IsNullOrWhiteSpace(text) || maxWidth <= 0) return new List<string> { "" };

        var lines = new List<string>();
        var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var currentLine = new StringBuilder();

        foreach (var word in words)
        {
            if (currentLine.Length > 0 && currentLine.Length + word.Length + 1 > maxWidth)
            {
                lines.Add(currentLine.ToString());
                currentLine.Clear();
            }

            if (currentLine.Length > 0)
                currentLine.Append(" ");

            currentLine.Append(word);
        }

        if (currentLine.Length > 0)
            lines.Add(currentLine.ToString());

        return lines.Any() ? lines : new List<string> { "" };
    }


}

public class ErrorImpl : Error
{
    public ErrorImpl(OutcomeType type, string code, string message, Error? innerError = null)
    {
        _type = type;
        _code = code;
        _message = message;
        if (innerError is not null) SetInnerError(innerError);
    }

    private OutcomeType _type;
    public override OutcomeType Type => _type;

    private string _code;
    public override string Code => _code;

    private string _message;
    public override string Message => _message;
}

public class ValidationError : ErrorImpl
{
    public ValidationError(Dictionary<string, string> fieldErrors, string message = "A validation error has occurred.", string code = "Default.Validation", Error? innerError = null) : base(OutcomeType.Validation, code, message, innerError)
    {
        FieldErrors = fieldErrors;
    }

    public Dictionary<string, string> FieldErrors { get; private set; }
}

public class NotFoundError : ErrorImpl
{
    public NotFoundError(string itemName, object identifier, string? message = null, string? code = null, Error? innerError = null)
        : base(OutcomeType.NotFound, code ?? $"NotFound.{itemName}", message ?? $"{itemName} with identifier '{identifier}' was not found.", innerError)
    {
        EntityName = itemName;
        Identifier = identifier;
    }

    public string EntityName { get; private set; }
    public object Identifier { get; private set; }
}

public enum ConflictType
{
    /// <summary>
    /// A resource with the same unique identifier already exists.
    /// (e.g., duplicate email or username).
    /// </summary>
    Duplicate,

    /// <summary>
    /// The resource has been modified by another process since it was last read
    /// (optimistic concurrency failure).
    /// </summary>
    StaleState,

    /// <summary>
    /// The requested operation violates a business rule related to the resource's current state
    /// (e.g., trying to cancel an order that has already been shipped).
    /// </summary>
    BusinessRuleViolation
}

public class ConflictError : ErrorImpl
{
    public ConflictError(
        ConflictType type,
        string resourceType,
        IReadOnlyDictionary<string, object> conflictingParameters,
        string? message = null,
        string? code = null,
        Error? innerError = null)
        : base(OutcomeType.Conflict, code ?? $"Conflict.{type}", message ?? $"A {type} conflict occurred on resource '{resourceType}'.", innerError)
    {
        Type = type;
        ResourceType = resourceType;
        ConflictingParameters = conflictingParameters;
    }

    public new ConflictType Type { get; private set; }
    public string ResourceType { get; private set; }
    public IReadOnlyDictionary<string, object> ConflictingParameters { get; private set; }
}


public class UnexpectedError : ErrorImpl
{
    public UnexpectedError(Exception exception, string? message = null, string? code = null)
        : base(OutcomeType.Unexpected, code ?? "System.Exception", message ?? exception.Message, null)
    {
        Exception = exception;

        if (exception.InnerException is not null)
        {
            SetInnerError(new UnexpectedError(exception.InnerException));
        }
    }

    public Exception Exception { get; private set; }
}

public class AuthorizationError : ErrorImpl
{
    public AuthorizationError(
        OutcomeType type,
        string action,
        string? resourceIdentifier,
        string? userId,
        string? message,
        string? code,
        Error? innerError)
        : base(type, code ?? $"Authorization.{type}", message ?? GenerateDefaultMessage(type, action, resourceIdentifier, userId), innerError)
    {
        UserId = userId;
        Action = action;
        ResourceIdentifier = resourceIdentifier;
    }

    /// <summary>
    /// The ID of the user who attempted the action. Can be null for anonymous users.
    /// </summary>
    public string? UserId { get; private set; }

    /// <summary>
    /// The action that was attempted (e.g., "DeleteProduct", "ViewInvoice").
    /// </summary>
    public string Action { get; private set; }

    /// <summary>
    /// An identifier for the resource being accessed. Can be null if the action is not resource-specific.
    /// </summary>
    public string? ResourceIdentifier { get; private set; }

    private static string GenerateDefaultMessage(OutcomeType type, string action, string? resourceIdentifier, string? userId)
    {
        var user = userId ?? "anonymous";
        if (resourceIdentifier is not null)
        {
            return $"User '{user}' is not authorized to perform action '{action}' on resource '{resourceIdentifier}'.";
        }
        return $"User '{user}' is not authorized to perform action '{action}'.";
    }
}

public class FailureError : ErrorImpl
{
    public FailureError(
        string message,
        string code,
        IReadOnlyDictionary<string, object>? contextData,
        Error? innerError)
        : base(OutcomeType.Failure, code, message, innerError)
    {
        ContextData = contextData ?? new Dictionary<string, object>();
    }

    /// <summary>
    /// Provides additional contextual data about the failure for logging and debugging.
    /// </summary>
    public IReadOnlyDictionary<string, object> ContextData { get; private set; }
}
