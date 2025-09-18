namespace Maybe;

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

    public AuthorizationError(Error error) : this()
    {
        SetInnerError(error);
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

