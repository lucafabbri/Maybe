namespace Maybe;

public static partial class MaybeExtensions
{
    /// <summary>
    /// If the outcome is a success, applies a mapping function to the value, returning a new Maybe with the transformed value.
    /// If the outcome is an error, the error is propagated.
    /// </summary>
    public static Maybe<TNewValue, TError> Select<TValue, TError, TNewValue>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, TNewValue> selector)
        where TError : IError
    {
        return maybe.IsSuccess
            ? Maybe<TNewValue, TError>.Some(selector(maybe.ValueOrThrow()))
            : Maybe<TNewValue, TError>.None(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// If the outcome is a success, applies a mapping function to the value, returning a new Maybe with the transformed value.
    /// </summary>
    public static Maybe<TNewValue, Error> Select<TValue, TNewValue>(
        this in Maybe<TValue> maybe,
        Func<TValue, TNewValue> selector)
    {
        Maybe<TValue, Error> fullMaybe = maybe;
        var result = fullMaybe.Select(selector);
        return result.IsSuccess
            ? Maybe<TNewValue>.Some(result.ValueOrThrow())
            : Maybe<TNewValue>.None(result.ErrorOrThrow());
    }

    /// <summary>
    /// Asynchronously applies a mapping function to the value of a Maybe, returning a new Maybe with the transformed value.
    /// </summary>
    public static async Task<Maybe<TNewValue, TError>> Select<TValue, TError, TNewValue>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, TNewValue> selector)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Select(selector);
    }

    /// <summary>
    /// Asynchronously applies a mapping function to the value of a Maybe, returning a new Maybe with the transformed value.
    /// </summary>
    public static async Task<Maybe<TNewValue, Error>> Select<TValue, TNewValue>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, TNewValue> selector)
    {
        Maybe<TValue, Error> fullMaybe = await maybeTask.ConfigureAwait(false);
        return fullMaybe.Select(selector);
    }

    /// <summary>
    /// Asynchronously applies an asynchronous mapping function to the value of a Maybe, returning a new Maybe with the transformed value.
    /// </summary>
    public static async Task<Maybe<TNewValue, TError>> SelectAsync<TValue, TError, TNewValue>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, Task<TNewValue>> selector)
        where TError : IError
    {
        return maybe.IsSuccess
            ? Maybe<TNewValue, TError>.Some(await selector(maybe.ValueOrThrow()).ConfigureAwait(false))
            : Maybe<TNewValue, TError>.None(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Asynchronously applies an asynchronous mapping function to the value of a Maybe, returning a new Maybe with the transformed value.
    /// </summary>
    public static async Task<Maybe<TNewValue, Error>> SelectAsync<TValue, TNewValue>(
        this Maybe<TValue> maybe,
        Func<TValue, Task<TNewValue>> selector)
    {
        Maybe<TValue, Error> fullMaybe = maybe;
        var result = await fullMaybe.SelectAsync(selector).ConfigureAwait(false);
        return result.IsSuccess
            ? Maybe<TNewValue>.Some(result.ValueOrThrow())
            : Maybe<TNewValue>.None(result.ErrorOrThrow());
    }

    /// <summary>
    /// Asynchronously applies an asynchronous mapping function to the value of a Maybe, returning a new Maybe with the transformed value.
    /// </summary>
    public static async Task<Maybe<TNewValue, TError>> SelectAsync<TValue, TError, TNewValue>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<TNewValue>> selector)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.SelectAsync(selector).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously applies an asynchronous mapping function to the value of a Maybe, returning a new Maybe with the transformed value.
    /// </summary>
    public static async Task<Maybe<TNewValue, TError>> SelectAsync<TValue, TError, TNewValue>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, Task<TNewValue>> selector)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        Maybe<TValue, TError> fullMaybe = maybe.IsSuccess
            ? Maybe<TValue, TError>.Some(maybe.ValueOrThrow())
            : Maybe<TValue, TError>.None((TError)maybe.ErrorOrThrow());
        return await fullMaybe.SelectAsync(selector).ConfigureAwait(false);
    }

    public static async Task<Maybe<TNewValue, Error>> SelectAsync<TValue, TNewValue>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, Task<TNewValue>> selector)
    {
        Maybe<TValue, Error> fullMaybe = await maybeTask.ConfigureAwait(false);
        return await fullMaybe.SelectAsync(selector).ConfigureAwait(false);
    }
}

