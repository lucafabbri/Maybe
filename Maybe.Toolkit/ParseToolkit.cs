using System.Globalization;
using Maybe;

namespace Maybe.Toolkit;

/// <summary>
/// Provides Maybe-based wrapper methods for common parsing operations.
/// </summary>
public static class ParseToolkit
{
    /// <summary>
    /// Attempts to parse a string to an integer, returning a Maybe result.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="style">Optional number style.</param>
    /// <param name="provider">Optional format provider.</param>
    /// <returns>A Maybe containing the parsed integer or a ParseError.</returns>
    public static Maybe<int, ParseError> TryParseInt(string value, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ParseError(value ?? "", typeof(int), null, "Value cannot be null or empty");
        }

        try
        {
            var result = int.Parse(value, style, provider ?? CultureInfo.InvariantCulture);
            return Maybe<int, ParseError>.Some(result);
        }
        catch (FormatException ex)
        {
            return new ParseError(value, typeof(int), ex, $"'{value}' is not a valid integer format");
        }
        catch (OverflowException ex)
        {
            return new ParseError(value, typeof(int), ex, $"'{value}' is too large or too small for an integer");
        }
        catch (Exception ex)
        {
            return new ParseError(value, typeof(int), ex, $"Unexpected error parsing '{value}' to integer");
        }
    }

    /// <summary>
    /// Attempts to parse a string to a long, returning a Maybe result.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="style">Optional number style.</param>
    /// <param name="provider">Optional format provider.</param>
    /// <returns>A Maybe containing the parsed long or a ParseError.</returns>
    public static Maybe<long, ParseError> TryParseLong(string value, NumberStyles style = NumberStyles.Integer, IFormatProvider? provider = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ParseError(value ?? "", typeof(long), null, "Value cannot be null or empty");
        }

        try
        {
            var result = long.Parse(value, style, provider ?? CultureInfo.InvariantCulture);
            return Maybe<long, ParseError>.Some(result);
        }
        catch (FormatException ex)
        {
            return new ParseError(value, typeof(long), ex, $"'{value}' is not a valid long format");
        }
        catch (OverflowException ex)
        {
            return new ParseError(value, typeof(long), ex, $"'{value}' is too large or too small for a long");
        }
        catch (Exception ex)
        {
            return new ParseError(value, typeof(long), ex, $"Unexpected error parsing '{value}' to long");
        }
    }

    /// <summary>
    /// Attempts to parse a string to a double, returning a Maybe result.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="style">Optional number style.</param>
    /// <param name="provider">Optional format provider.</param>
    /// <returns>A Maybe containing the parsed double or a ParseError.</returns>
    public static Maybe<double, ParseError> TryParseDouble(string value, NumberStyles style = NumberStyles.Float | NumberStyles.AllowThousands, IFormatProvider? provider = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ParseError(value ?? "", typeof(double), null, "Value cannot be null or empty");
        }

        try
        {
            var result = double.Parse(value, style, provider ?? CultureInfo.InvariantCulture);
            return Maybe<double, ParseError>.Some(result);
        }
        catch (FormatException ex)
        {
            return new ParseError(value, typeof(double), ex, $"'{value}' is not a valid double format");
        }
        catch (OverflowException ex)
        {
            return new ParseError(value, typeof(double), ex, $"'{value}' is too large or too small for a double");
        }
        catch (Exception ex)
        {
            return new ParseError(value, typeof(double), ex, $"Unexpected error parsing '{value}' to double");
        }
    }

    /// <summary>
    /// Attempts to parse a string to a decimal, returning a Maybe result.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="style">Optional number style.</param>
    /// <param name="provider">Optional format provider.</param>
    /// <returns>A Maybe containing the parsed decimal or a ParseError.</returns>
    public static Maybe<decimal, ParseError> TryParseDecimal(string value, NumberStyles style = NumberStyles.Number, IFormatProvider? provider = null)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ParseError(value ?? "", typeof(decimal), null, "Value cannot be null or empty");
        }

        try
        {
            var result = decimal.Parse(value, style, provider ?? CultureInfo.InvariantCulture);
            return Maybe<decimal, ParseError>.Some(result);
        }
        catch (FormatException ex)
        {
            return new ParseError(value, typeof(decimal), ex, $"'{value}' is not a valid decimal format");
        }
        catch (OverflowException ex)
        {
            return new ParseError(value, typeof(decimal), ex, $"'{value}' is too large or too small for a decimal");
        }
        catch (Exception ex)
        {
            return new ParseError(value, typeof(decimal), ex, $"Unexpected error parsing '{value}' to decimal");
        }
    }

    /// <summary>
    /// Attempts to parse a string to a Guid, returning a Maybe result.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <returns>A Maybe containing the parsed Guid or a ParseError.</returns>
    public static Maybe<Guid, ParseError> TryParseGuid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ParseError(value ?? "", typeof(Guid), null, "Value cannot be null or empty");
        }

        try
        {
            var result = Guid.Parse(value);
            return Maybe<Guid, ParseError>.Some(result);
        }
        catch (FormatException ex)
        {
            return new ParseError(value, typeof(Guid), ex, $"'{value}' is not a valid Guid format");
        }
        catch (Exception ex)
        {
            return new ParseError(value, typeof(Guid), ex, $"Unexpected error parsing '{value}' to Guid");
        }
    }

    /// <summary>
    /// Attempts to parse a string to a DateTime, returning a Maybe result.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <param name="provider">Optional format provider.</param>
    /// <param name="styles">Optional date time styles.</param>
    /// <returns>A Maybe containing the parsed DateTime or a ParseError.</returns>
    public static Maybe<DateTime, ParseError> TryParseDateTime(string value, IFormatProvider? provider = null, DateTimeStyles styles = DateTimeStyles.None)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ParseError(value ?? "", typeof(DateTime), null, "Value cannot be null or empty");
        }

        try
        {
            var result = DateTime.Parse(value, provider ?? CultureInfo.InvariantCulture, styles);
            return Maybe<DateTime, ParseError>.Some(result);
        }
        catch (FormatException ex)
        {
            return new ParseError(value, typeof(DateTime), ex, $"'{value}' is not a valid DateTime format");
        }
        catch (Exception ex)
        {
            return new ParseError(value, typeof(DateTime), ex, $"Unexpected error parsing '{value}' to DateTime");
        }
    }

    /// <summary>
    /// Attempts to parse a string to a boolean, returning a Maybe result.
    /// </summary>
    /// <param name="value">The string to parse.</param>
    /// <returns>A Maybe containing the parsed boolean or a ParseError.</returns>
    public static Maybe<bool, ParseError> TryParseBool(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return new ParseError(value ?? "", typeof(bool), null, "Value cannot be null or empty");
        }

        try
        {
            var result = bool.Parse(value);
            return Maybe<bool, ParseError>.Some(result);
        }
        catch (FormatException ex)
        {
            return new ParseError(value, typeof(bool), ex, $"'{value}' is not a valid boolean format");
        }
        catch (Exception ex)
        {
            return new ParseError(value, typeof(bool), ex, $"Unexpected error parsing '{value}' to boolean");
        }
    }
}