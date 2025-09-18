namespace Maybe;

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

    public ValidationError(Error error) : this([])
    {
        SetInnerError(error);
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

