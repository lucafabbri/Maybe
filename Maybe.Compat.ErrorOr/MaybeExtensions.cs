using ErrorOr;

namespace Maybe;

/// <summary>
/// Provides extension methods for the Maybe type for interoperability.
/// </summary>
public static class MaybeExtensions
{
    /// <summary>
    /// Converts a Maybe<TValue, TError> to an ErrorOr<TValue>.
    /// </summary>
    /// <remarks>
    /// If the Maybe is a success, the value is preserved.
    /// If the Maybe is an error, the hierarchical error chain is flattened into a list of ErrorOr.Error.
    /// </remarks>
    /// <typeparam name="TValue">The type of the success value.</typeparam>
    /// <typeparam name="TError">The type of the error in the Maybe.</typeparam>
    /// <param name="maybe">The Maybe instance to convert.</param>
    /// <returns>An ErrorOr<TValue> representing the outcome of the Maybe.</returns>
    public static ErrorOr<TValue> ToErrorOr<TValue, TError>(this Maybe<TValue, TError> maybe)
        where TError : Error, new()
    {
        if (maybe.IsSuccess)
        {
            return maybe.ValueOrThrow();
        }

        var errors = new List<ErrorOr.Error>();
        Error? currentError = maybe.ErrorOrThrow();

        while (currentError is not null)
        {
            var errorOrError = currentError.Type switch
            {
                OutcomeType.Validation => ErrorOr.Error.Validation(currentError.Code, currentError.Message),
                OutcomeType.NotFound => ErrorOr.Error.NotFound(currentError.Code, currentError.Message),
                OutcomeType.Conflict => ErrorOr.Error.Conflict(currentError.Code, currentError.Message),
                OutcomeType.Unauthorized => ErrorOr.Error.Unauthorized(currentError.Code, currentError.Message),
                OutcomeType.Unexpected => ErrorOr.Error.Unexpected(currentError.Code, currentError.Message),
                _ => ErrorOr.Error.Failure(currentError.Code, currentError.Message)
            };

            errors.Add(errorOrError);
            currentError = currentError.InnerError;
        }

        return errors;
    }
}
