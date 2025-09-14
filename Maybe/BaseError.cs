using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

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

/// <summary>
/// Represents the base class for all specialized errors in the system.
/// It provides common properties and functionality for error handling.
/// </summary>
public abstract class BaseError : IError
{
    private BaseError? _innerError;

    /// <summary>
    /// Gets the inner error, if any, that caused this error.
    /// This is used to preserve the error chain.
    /// </summary>
    public BaseError? InnerError => _innerError;

    /// <inheritdoc/>
    public abstract OutcomeType Type { get; protected set; }

    /// <inheritdoc/>
    public abstract string Code { get; protected set; }

    /// <inheritdoc/>
    public abstract string Message { get; protected set; }

    /// <inheritdoc/>
    public int TimeStamp { get; } = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseError"/> class.
    /// </summary>
    public BaseError()
    {
    }

    /// <summary>
    /// Sets the inner error for this error instance.
    /// </summary>
    /// <param name="innerError">The error that is the cause of the current error.</param>
    internal void SetInnerError(BaseError innerError)
    {
        _innerError = innerError;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj?.GetType() == GetType() && obj is BaseError other && Equals(other);
    }

    /// <inheritdoc/>
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
    /// The layout uses aligned columns and wraps long messages for readability, suitable for detailed logging.
    /// </summary>
    /// <returns>The formatted string of the entire error chain.</returns>
    public string ToFullString()
    {
        // 1. Collect all errors and their hierarchy depth
        var allErrors = new List<(int depth, BaseError error)>();
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
    private void CollectErrors(BaseError? current, int depth, List<(int depth, BaseError error)> list)
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
        if (string.IsNullOrWhiteSpace(text) || maxWidth <= 0) return [""];

        var lines = new List<string>();
        var words = text.Split([' '], StringSplitOptions.RemoveEmptyEntries);
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

        return lines.Any() ? lines : [""];
    }


}

/// <summary>
/// A generic, concrete implementation of the <see cref="BaseError"/> class.
/// Used for errors that do not have specialized properties.
/// </summary>
public class Error : BaseError
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with default values.
    /// This constructor is intended for scenarios requiring a parameterless constructor, like generic constraints.
    /// </summary>
    public Error()
    {
        Type = OutcomeType.Failure;
        Code = "Default.Error";
        Message = "An error has occurred.";
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class.
    /// </summary>
    /// <param name="type">The type of the error outcome.</param>
    /// <param name="code">A unique code identifying the error.</param>
    /// <param name="message">A descriptive message for the error.</param>
    /// <param name="innerError">The error that is the cause of the current error, if any.</param>
    public Error(OutcomeType type, string code, string message, BaseError? innerError = null)
    {
        Type = type;
        Code = code;
        Message = message;
        if (innerError is not null) SetInnerError(innerError);
    }

    /// <inheritdoc/>
    public override OutcomeType Type { get; protected set; }
    /// <inheritdoc/>
    public override string Code { get; protected set; }
    /// <inheritdoc/>
    public override string Message { get; protected set; }


    #region Static Factory Methods

    /// <summary>
    /// Creates a custom error with the specified parameters.
    /// </summary>
    /// <param name="type">The type of the error outcome.</param>
    /// <param name="code">A unique code identifying the error.</param>
    /// <param name="message">A descriptive message for the error.</param>
    /// <param name="innerError">The error that is the cause of the current error, if any.</param>
    /// <returns>A new <see cref="BaseError"/> instance.</returns>
    public static Error Custom(OutcomeType type, string code, string message, BaseError? innerError = null) =>
        new Error(type, code, message, innerError);

    /// <summary>
    /// Creates a new <see cref="FailureError"/>.
    /// This represents an expected, plausible, but significant failure in a process.
    /// </summary>
    /// <param name="message">A descriptive message for the failure.</param>
    /// <param name="code">A unique code identifying the failure.</param>
    /// <param name="contextData">A dictionary of contextual data for debugging.</param>
    /// <param name="innerError">The error that is the cause of the current error, if any.</param>
    /// <returns>A new <see cref="FailureError"/> instance.</returns>
    public static FailureError Failure(
        string message = "A failure has occurred.",
        string code = "Default.Failure",
        Dictionary<string, object>? contextData = null,
        BaseError? innerError = null)
        => new FailureError(message, code, contextData, innerError);

    /// <summary>
    /// Creates a new <see cref="UnexpectedError"/> from an exception.
    /// This represents an unhandled or system-level exception.
    /// </summary>
    /// <param name="exception">The exception that caused the error.</param>
    /// <param name="message">An optional custom message. If null, the exception's message is used.</param>
    /// <param name="code">An optional custom code. If null, "System.Exception" is used.</param>
    /// <returns>A new <see cref="UnexpectedError"/> instance.</returns>
    public static UnexpectedError Unexpected(Exception exception, string? message = null, string? code = null) =>
        new UnexpectedError(exception, message, code);

    /// <summary>
    /// Creates a new <see cref="ValidationError"/>.
    /// This represents an error due to invalid input data.
    /// </summary>
    /// <param name="fieldsErrors">A dictionary containing field-specific error messages.</param>
    /// <param name="message">A general message for the validation failure.</param>
    /// <param name="code">A unique code for the validation failure.</param>
    /// <param name="innerError">The error that is the cause of the current error, if any.</param>
    /// <returns>A new <see cref="ValidationError"/> instance.</returns>
    public static ValidationError Validation(Dictionary<string, string> fieldsErrors, string message = "A validation error has occurred.", string code = "Default.Validation", BaseError? innerError = null) =>
        new ValidationError(fieldsErrors, message, code, innerError);

    /// <summary>
    /// Creates a new <see cref="ConflictError"/>.
    /// This represents a conflict with the current state of a resource.
    /// </summary>
    /// <param name="type">The specific type of conflict.</param>
    /// <param name="resourceType">The type of the resource that has the conflict (e.g., "User").</param>
    /// <param name="conflictingParameters">A dictionary of parameters that caused the conflict.</param>
    /// <param name="message">An optional custom message for the conflict.</param>
    /// <param name="code">An optional custom code for the conflict.</param>
    /// <param name="innerError">The error that is the cause of the current error, if any.</param>
    /// <returns>A new <see cref="ConflictError"/> instance.</returns>
    public static ConflictError Conflict(
        ConflictType type,
        string resourceType,
        Dictionary<string, object> conflictingParameters,
        string? message = null,
        string? code = null,
        BaseError? innerError = null) => new ConflictError(type, resourceType, conflictingParameters, message, code, innerError);

    /// <summary>
    /// Creates a new <see cref="NotFoundError"/>.
    /// This represents a failure to find a requested resource.
    /// </summary>
    /// <param name="itemName">The name of the entity that was not found (e.g., "Product").</param>
    /// <param name="identifier">The identifier used to search for the entity.</param>
    /// <param name="message">An optional custom message.</param>
    /// <param name="code">An optional custom code.</param>
    /// <param name="innerError">The error that is the cause of the current error, if any.</param>
    /// <returns>A new <see cref="NotFoundError"/> instance.</returns>
    public static NotFoundError NotFound(string itemName, object identifier, string? message = null, string? code = null, BaseError? innerError = null) =>
        new NotFoundError(itemName, identifier, message, code, innerError);

    /// <summary>
    /// Creates a new <see cref="AuthorizationError"/> with an <see cref="OutcomeType.Unauthorized"/> type.
    /// This indicates that the user is not authenticated or lacks the required credentials for the action.
    /// </summary>
    /// <param name="action">The action that was attempted (e.g., "UpdateProfile").</param>
    /// <param name="resourceIdentifier">An optional identifier for the resource being accessed.</param>
    /// <param name="userId">The ID of the user who attempted the action. Can be null for anonymous users.</param>
    /// <param name="message">An optional custom message.</param>
    /// <param name="code">An optional custom code.</param>
    /// <param name="innerError">The error that is the cause of the current error, if any.</param>
    /// <returns>A new <see cref="AuthorizationError"/> instance.</returns>
    public static AuthorizationError Unauthorized(
        string action,
        string? resourceIdentifier = null,
        string? userId = null,
        string? message = null,
        string? code = null,
        BaseError? innerError = null)
        => new AuthorizationError(OutcomeType.Unauthorized, action, resourceIdentifier, userId, message, code, innerError);

    /// <summary>
    /// Creates a new <see cref="AuthorizationError"/> with an <see cref="OutcomeType.Forbidden"/> type.
    /// This indicates that the user is authenticated but does not have permission to perform the action.
    /// </summary>
    /// <param name="action">The action that was attempted (e.g., "DeleteUser").</param>
    /// <param name="resourceIdentifier">An optional identifier for the resource being accessed.</param>
    /// <param name="userId">The ID of the user who attempted the action.</param>
    /// <param name="message">An optional custom message.</param>
    /// <param name="code">An optional custom code.</param>
    /// <param name="innerError">The error that is the cause of the current error, if any.</param>
    /// <returns>A new <see cref="AuthorizationError"/> instance.</returns>
    public static AuthorizationError Forbidden(
        string action,
        string? resourceIdentifier = null,
        string? userId = null,
        string? message = null,
        string? code = null,
        BaseError? innerError = null)
        => new AuthorizationError(OutcomeType.Forbidden, action, resourceIdentifier, userId, message, code, innerError);

    #endregion
}

/// <summary>
/// Represents a validation error, containing details about which fields failed validation.
/// </summary>
public class ValidationError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class with default values.
    /// </summary>
    public ValidationError() : base(OutcomeType.Validation, "Default.Validation", "A validation error has occurred.")
    {
        FieldErrors = [];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class.
    /// </summary>
    /// <param name="fieldErrors">A dictionary containing field-specific error messages.</param>
    /// <param name="message">A general message for the validation failure.</param>
    /// <param name="code">A unique code for the validation failure.</param>
    /// <param name="innerError">The error that is the cause of the current error, if any.</param>
    public ValidationError(
        Dictionary<string, string> fieldErrors,
        string message = "A validation error has occurred.",
        string code = "Default.Validation",
        BaseError? innerError = null) : base(OutcomeType.Validation, code, message, innerError)
    {
        FieldErrors = fieldErrors;
    }

    /// <summary>
    /// Gets or sets a dictionary containing field-specific error messages. The key is the field name, and the value is the error message.
    /// </summary>
    public Dictionary<string, string> FieldErrors { get; set; }
}

/// <summary>
/// Represents an error that occurs when a requested entity or resource is not found.
/// </summary>
public class NotFoundError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundError"/> class with default values.
    /// </summary>
    public NotFoundError() : base(OutcomeType.NotFound, "Default.NotFound", "A 'not found' error has occurred.")
    {
        EntityName = string.Empty;
        Identifier = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundError"/> class.
    /// </summary>
    /// <param name="itemName">The name of the entity that was not found (e.g., "User").</param>
    /// <param name="identifier">The identifier used to search for the entity (e.g., a user ID).</param>
    /// <param name="message">An optional custom message. A default message is generated if not provided.</param>
    /// <param name="code">An optional custom code. A default code is generated if not provided.</param>
    /// <param name="innerError">The error that is the cause of the current error, if any.</param>
    public NotFoundError(
        string itemName,
        object identifier,
        string? message = null,
        string? code = null,
        BaseError? innerError = null)
        : base(OutcomeType.NotFound, code ?? $"NotFound.{itemName}", message ?? $"{itemName} with identifier '{identifier}' was not found.", innerError)
    {
        EntityName = itemName;
        Identifier = identifier;
    }

    /// <summary>
    /// Gets or sets the name of the entity that was not found.
    /// </summary>
    public string EntityName { get; set; }

    /// <summary>
    /// Gets or sets the identifier used to search for the entity.
    /// </summary>
    public object Identifier { get; set; }
}


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
        Type = ConflictType.BusinessRuleViolation;
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
        Type = type;
        ResourceType = resourceType;
        ConflictingParameters = conflictingParameters;
    }

    /// <summary>
    /// Gets or sets the specific type of conflict.
    /// </summary>
    public new ConflictType Type { get; set; }

    /// <summary>
    /// Gets or sets the type of the resource that has the conflict (e.g., "Order", "Product").
    /// </summary>
    public string ResourceType { get; set; }

    /// <summary>
    /// Gets or sets a dictionary of parameters and values that caused the conflict.
    /// </summary>
    public IReadOnlyDictionary<string, object> ConflictingParameters { get; set; }
}

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

/// <summary>
/// Represents an authorization error, indicating that a user is either not authenticated (<see cref="OutcomeType.Unauthorized"/>)
/// or not permitted to perform an action (<see cref="OutcomeType.Forbidden"/>).
/// </summary>
public class AuthorizationError : Error
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationError"/> class with default values.
    /// </summary>
    public AuthorizationError() : base(OutcomeType.Unauthorized, "Authorization.Unauthorized", GenerateDefaultMessage(OutcomeType.Unauthorized, string.Empty, null, null))
    {
        Action = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationError"/> class.
    /// </summary>
    /// <param name="type">The type of authorization error (<see cref="OutcomeType.Unauthorized"/> or <see cref="OutcomeType.Forbidden"/>).</param>
    /// <param name="action">The action that was attempted.</param>
    /// <param name="resourceIdentifier">An optional identifier for the resource being accessed.</param>
    /// <param name="userId">The ID of the user who attempted the action.</param>
    /// <param name="message">An optional custom message.</param>
    /// <param name="code">An optional custom code.</param>
    /// <param name="innerError">The error that is the cause of the current error, if any.</param>
    public AuthorizationError(
        OutcomeType type,
        string action,
        string? resourceIdentifier = null,
        string? userId = null,
        string? message = null,
        string? code = null,
        BaseError? innerError = null)
        : base(type, code ?? $"Authorization.{type}", message ?? GenerateDefaultMessage(type, action, resourceIdentifier, userId), innerError)
    {
        UserId = userId;
        Action = action;
        ResourceIdentifier = resourceIdentifier;
    }

    /// <summary>
    /// Gets or sets the ID of the user who attempted the action. Can be null for anonymous users.
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Gets or sets the action that was attempted (e.g., "DeleteProduct", "ViewInvoice").
    /// </summary>
    public string Action { get; set; }

    /// <summary>
    /// Gets or sets an identifier for the resource being accessed. Can be null if the action is not resource-specific.
    /// </summary>
    public string? ResourceIdentifier { get; set; }

    /// <summary>
    /// Generates a default error message based on the provided details.
    /// </summary>
    private static string GenerateDefaultMessage(OutcomeType type, string action, string? resourceIdentifier, string? userId)
    {
        var user = userId ?? "anonymous";
        if (string.IsNullOrEmpty(action))
        {
            return $"User '{user}' is not authorized to perform this action.";
        }
        if (resourceIdentifier is not null)
        {
            return $"User '{user}' is not authorized to perform action '{action}' on resource '{resourceIdentifier}'.";
        }
        return $"User '{user}' is not authorized to perform action '{action}'.";
    }
}

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
        ContextData = new Dictionary<string, object>();
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
        IReadOnlyDictionary<string, object>? contextData = null,
        BaseError? innerError = null)
        : base(OutcomeType.Failure, code, message, innerError)
    {
        ContextData = contextData ?? new Dictionary<string, object>();
    }

    /// <summary>
    /// Gets or sets additional contextual data about the failure for logging and debugging.
    /// This can include parameters, state, or other information relevant to the failed process.
    /// </summary>
    public IReadOnlyDictionary<string, object> ContextData { get; set; }
}

