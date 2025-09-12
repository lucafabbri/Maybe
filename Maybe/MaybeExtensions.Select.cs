namespace Maybe;

/// <summary>
/// Provides extension methods for transforming the success value of a Maybe (also known as Map).
/// </summary>
public static partial class MaybeExtensions
{
    #region Synchronous Select (Sync Selector)

    /// <summary>
    /// If the outcome is a success, applies a mapping function to the value, returning a new Maybe with the transformed value
    /// while preserving the original error type.
    /// </summary>
    public static Maybe<TResult, TError> Select<TValue, TError, TResult>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, TResult> selector)
        where TError : Error
    {
        return maybe.IsSuccess
            ? Maybe<TResult, TError>.Some(selector(maybe.ValueOrThrow()))
            : Maybe<TResult, TError>.None(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// If the outcome is a success, applies a mapping function to the value. Works with the default Error type.
    /// </summary>
    public static Maybe<TResult> Select<TValue, TResult>(
        this in Maybe<TValue> maybe,
        Func<TValue, TResult> selector)
    {
        return maybe.IsSuccess
            ? Maybe<TResult>.Some(selector(maybe.ValueOrThrow()))
            : Maybe<TResult>.None(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then applies a synchronous mapping function to its value.
    /// </summary>
    public static async Task<Maybe<TResult, TError>> Select<TValue, TError, TResult>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, TResult> selector)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Select(selector);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then applies a synchronous mapping function to its value.
    /// </summary>
    public static async Task<Maybe<TResult>> Select<TValue, TResult>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, TResult> selector)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Select(selector);
    }

    #endregion

    #region Asynchronous Select (Async Selector)

    /// <summary>
    /// If the outcome is a success, applies an asynchronous mapping function to the value.
    /// </summary>
    public static async Task<Maybe<TResult, TError>> SelectAsync<TValue, TError, TResult>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, Task<TResult>> selectorAsync)
        where TError : Error
    {
        if (maybe.IsError)
        {
            return Maybe<TResult, TError>.None(maybe.ErrorOrThrow());
        }
        var result = await selectorAsync(maybe.ValueOrThrow()).ConfigureAwait(false);
        return Maybe<TResult, TError>.Some(result);
    }

    /// <summary>
    /// If the outcome is a success, applies an asynchronous mapping function to the value.
    /// </summary>
    public static async Task<Maybe<TResult>> SelectAsync<TValue, TResult>(
        this Maybe<TValue> maybe,
        Func<TValue, Task<TResult>> selectorAsync)
    {
        if (maybe.IsError)
        {
            return Maybe<TResult>.None(maybe.ErrorOrThrow());
        }
        var result = await selectorAsync(maybe.ValueOrThrow()).ConfigureAwait(false);
        return Maybe<TResult>.Some(result);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then applies an asynchronous mapping function to its value.
    /// </summary>
    public static async Task<Maybe<TResult, TError>> SelectAsync<TValue, TError, TResult>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<TResult>> selectorAsync)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.SelectAsync(selectorAsync).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then applies an asynchronous mapping function to its value.
    /// </summary>
    public static async Task<Maybe<TResult>> SelectAsync<TValue, TResult>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, Task<TResult>> selectorAsync)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.SelectAsync(selectorAsync).ConfigureAwait(false);
    }

    #endregion
}

