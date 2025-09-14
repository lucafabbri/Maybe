namespace Maybe;

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

