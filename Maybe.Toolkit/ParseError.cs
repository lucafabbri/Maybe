using Maybe;

namespace Maybe.Toolkit;

/// <summary>
/// Error for parsing operations.
/// </summary>
public class ParseError : ValidationError
{
    public override OutcomeType Type => OutcomeType.Validation;
    public override string Code => "Parse.FormatError";
    public override string Message => "Parsing operation failed.";

    /// <summary>
    /// The value that failed to parse.
    /// </summary>
    public string? InputValue { get; private set; }

    /// <summary>
    /// The target type that was being parsed to.
    /// </summary>
    public Type? TargetType { get; private set; }

    /// <summary>
    /// The original exception that caused the parse error.
    /// </summary>
    public Exception? OriginalException { get; private set; }

    public ParseError() { }

    public ParseError(string inputValue, Type targetType, Exception? originalException = null, string? customMessage = null)
    {
        InputValue = inputValue;
        TargetType = targetType;
        OriginalException = originalException;
        if (customMessage != null)
        {
            Message = customMessage;
        }
    }
}