namespace Maybe;

/// <summary>
/// Provides a default, concrete implementation of IError for common scenarios.
/// </summary>
public class Error : IEquatable<Error>
{
    /// <inheritdoc/>
    public OutcomeType Type { get; protected set; }

    /// <inheritdoc/>
    public string Code { get; protected set; }

    /// <inheritdoc/>
    public string Message { get; protected set; }

    /// <inheritdoc/>
    public int TimeStamp { get; } = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    public Error(Error other)
    {
        if(other == null) throw new ArgumentNullException(nameof(other));
        Type = other.Type;
        Code = other.Code;
        Message = other.Message;
    }

    protected Error(OutcomeType type, string code, string message)
    {
        Type = type;
        Code = code;
        Message = message;
    }

    // Static factory methods for creating default errors with clear intent.

    public static Error Custom(OutcomeType type, string code, string message) =>
        new(type, code, message);

    public static Error Failure(string code = "Default.Failure", string message = "A failure has occurred.") =>
        new(OutcomeType.Failure, code, message);

    public static Error Unexpected(string code = "Default.Unexpected", string message = "An unexpected error has occurred.") =>
        new(OutcomeType.Unexpected, code, message);

    public static Error Validation(string code = "Default.Validation", string message = "A validation error has occurred.") =>
        new(OutcomeType.Validation, code, message);

    public static Error Conflict(string code = "Default.Conflict", string message = "A conflict error has occurred.") =>
        new(OutcomeType.Conflict, code, message);

    public static Error NotFound(string code = "Default.NotFound", string message = "A 'not found' error has occurred.") =>
        new(OutcomeType.NotFound, code, message);

    public static Error Unauthorized(string code = "Default.Unauthorized", string message = "An unauthorized error has occurred.") =>
        new(OutcomeType.Unauthorized, code, message);

    public static Error Forbidden(string code = "Default.Forbidden", string message = "A forbidden error has occurred.") =>
        new(OutcomeType.Forbidden, code, message);

    public virtual bool Equals(Error other)
    {
        if (other is null) return false;
        return Type == other.Type && Code == other.Code;
    }

    public override bool Equals(object? obj)
    {
        return obj?.GetType() == GetType() && obj is Error other && Equals(other);
    }
}
