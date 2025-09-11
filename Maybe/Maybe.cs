using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Maybe;

/// <summary>
/// A simplified version of Maybe that uses a Error type for failures.
/// This provides a cleaner API for scenarios where a custom error type is not needed.
/// </summary>
/// <typeparam name="TValue">The type of the success value.</typeparam>
public readonly struct Maybe<TValue>
{
    private readonly Maybe<TValue, Error> _innerMaybe;

    private Maybe(TValue value) => _innerMaybe = Maybe<TValue, Error>.Some(value);
    private Maybe(Error error) => _innerMaybe = Maybe<TValue, Error>.None(error);

    /// <summary>
    /// Gets a value indicating whether the outcome is a success.
    /// </summary>
    public bool IsSuccess => _innerMaybe.IsSuccess;

    /// <summary>
    /// Gets a value indicating whether the outcome is an error.
    /// </summary>
    public bool IsError => _innerMaybe.IsError;

    /// <summary>
    /// Gets the type of the outcome.
    /// </summary>
    public OutcomeType Type => _innerMaybe.Type;

    // --- Unsafe Accessors ---
    public TValue ValueOrThrow(string? message = null) => _innerMaybe.ValueOrThrow(message);
    public Error ErrorOrThrow(string? message = null) => _innerMaybe.ErrorOrThrow(message);

    // --- Safe Fallbacks ---
    public TValue? ValueOrDefault() => _innerMaybe.ValueOrDefault();

    public static Maybe<TValue> Some(TValue value) => new(value);

    public static Maybe<TValue> None(Error error) => new(error);

    // --- Implicit Conversions for seamless interoperability ---

    /// <summary>
    /// Implicitly converts a value to a successful Maybe.
    /// </summary>
    public static implicit operator Maybe<TValue>(TValue value) => new(value);

    /// <summary>
    /// Implicitly converts a Error to a failed Maybe.
    /// </summary>
    public static implicit operator Maybe<TValue>(Error error) => new(error);
    
    public static implicit operator Maybe<TValue, Error>(Maybe<TValue> maybe) => maybe._innerMaybe;

}

/// <summary>
/// Represents the outcome of an operation, which can be either a success value or an error.
/// </summary>
/// <typeparam name="TValue">The type of the success value.</typeparam>
/// <typeparam name="TError">The type of the error, which must implement IError.</typeparam>
public readonly struct Maybe<TValue, TError>
    where TError : Error
{
    private readonly TValue _value;
    private readonly TError _error;
    private readonly bool _isSuccess;

    /// <summary>
    /// Gets a value indicating whether the outcome is a success.
    /// </summary>
#if NET8_0_OR_GREATER
    [MemberNotNullWhen(true, "ValueOrThrow")]
    [MemberNotNullWhen(false, "ErrorOrThrow")]
#endif
    public bool IsSuccess => _isSuccess;

    /// <summary>
    /// Gets a value indicating whether the outcome is an error.
    /// </summary>
    public bool IsError => !_isSuccess;

    /// <summary>
    /// Gets the success value, or throws an <see cref="InvalidOperationException"/> if the outcome is an error.
    /// This makes the unsafe access explicit and deliberate.
    /// </summary>
    /// <param name="message">An optional message to include in the exception.</param>
    /// <returns>The success value.</returns>
    public TValue ValueOrThrow(string? message = null) =>
        _isSuccess ? _value : throw new InvalidOperationException(message ?? "Cannot access the value of an error outcome.");

    /// <summary>
    /// Gets the error, or throws an <see cref="InvalidOperationException"/> if the outcome is a success.
    /// </summary>
    /// <param name="message">An optional message to include in the exception.</param>
    /// <returns>The error object.</returns>
    public TError ErrorOrThrow(string? message = null) =>
        !_isSuccess ? _error : throw new InvalidOperationException(message ?? "Cannot access the error of a success outcome.");

    /// <summary>
    /// Gets the success value, or the default value for <typeparamref name="TValue"/> if the outcome is an error.
    /// For value types, this returns the language default (e.g., 0 for int). For reference types, this will be null.
    /// </summary>
    /// <returns>The success value or the type's default value.</returns>
    public TValue? ValueOrDefault() => _isSuccess ? _value : default(TValue);

    /// <summary>
    /// Gets the error, or null if the outcome is a success. For errors, this is equivalent to <see cref="ErrorOrNull"/>.
    /// </summary>
    /// <returns>The error object or null.</returns>
    public TError? ErrorOrDefault() => !_isSuccess ? _error : default;

    /// <summary>
    /// Gets the type of the outcome.
    /// If the outcome is a success and the value implements IOutcome, its Type is returned.
    /// Otherwise, a successful outcome defaults to OutcomeType.Success.
    /// For an error outcome, the error's Type is returned.
    /// </summary>
    public OutcomeType Type
    {
        get
        {
            if (!_isSuccess)
            {
                return _error.Type;
            }

            if (_value is IOutcome outcomeValue)
            {
                return outcomeValue.Type;
            }

            return OutcomeType.Success; // Default for success
        }
    }

    private Maybe(TValue value)
    {
        _isSuccess = true;
        _value = value;
        _error = default!;
    }

    private Maybe(TError error)
    {
        _isSuccess = false;
        _value = default!;
        _error = error;
    }

    /// <summary>
    /// Creates a success outcome with the specified value.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TValue, TError> Some(TValue value) => new(value);

    /// <summary>
    /// Creates an error outcome with the specified error.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TValue, TError> None(TError error) => new(error);

    public static Maybe<TValue, TError> None(Error error)
    {
        if (error is TError typedError)
        {
            return new Maybe<TValue, TError>(typedError);
        }
        else
        {
            var builtError = (TError)Activator.CreateInstance(typeof(TError),error)!;
            return new Maybe<TValue, TError>(builtError);
        }

    }

    /// <summary>
    /// Implicitly converts a success value to a Maybe outcome.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Maybe<TValue, TError>(TValue value) => Some(value);

    /// <summary>
    /// Implicitly converts an error to a Maybe outcome.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Maybe<TValue, TError>(TError error) => None(error);

    public static explicit operator Maybe<TValue, TError>(Error error) => None(error);
}

