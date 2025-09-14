namespace Maybe;

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

