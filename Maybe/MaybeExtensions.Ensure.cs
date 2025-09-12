namespace Maybe;

/// <summary>
/// Provides extension methods for validating the success value of a Maybe.
/// </summary>
public static partial class MaybeExtensions
{
    #region Synchronous Ensure (Sync Predicate)

    /// <summary>
    /// If the outcome is a success, checks if the value satisfies a predicate. If not, it returns the provided error.
    /// This method unifies the error channel to the base 'Error' type for type safety in chained calls.
    /// </summary>
    public static Maybe<TValue> Ensure<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, bool> predicate,
        Error error)
        where TError : Error
    {
        if (maybe.IsError)
        {
            return maybe.ErrorOrThrow();
        }

        return predicate(maybe.ValueOrThrow()) ? maybe.ValueOrThrow() : error;
    }

    /// <summary>
    /// If the outcome is a success, checks if the value satisfies a predicate. If not, it returns the provided error.
    /// </summary>
    public static Maybe<TValue> Ensure<TValue>(
        this in Maybe<TValue> maybe,
        Func<TValue, bool> predicate,
        Error error)
    {
        if (maybe.IsError)
        {
            return maybe.ErrorOrThrow();
        }

        return predicate(maybe.ValueOrThrow()) ? maybe.ValueOrThrow() : error;
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then applies the Ensure validation.
    /// </summary>
    public static async Task<Maybe<TValue>> Ensure<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, bool> predicate,
        Error error)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Ensure(predicate, error);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then applies the Ensure validation.
    /// </summary>
    public static async Task<Maybe<TValue>> Ensure<TValue>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, bool> predicate,
        Error error)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Ensure(predicate, error);
    }

    #endregion

    #region Asynchronous Ensure (Async Predicate)

    /// <summary>
    /// If the outcome is a success, asynchronously checks if the value satisfies a predicate. If not, returns the provided error.
    /// </summary>
    public static async Task<Maybe<TValue>> EnsureAsync<TValue, TError>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, Task<bool>> predicateAsync,
        Error error)
        where TError : Error
    {
        if (maybe.IsError)
        {
            return maybe.ErrorOrThrow();
        }

        return await predicateAsync(maybe.ValueOrThrow()).ConfigureAwait(false)
            ? maybe.ValueOrThrow()
            : error;
    }

    /// <summary>
    /// If the outcome is a success, asynchronously checks if the value satisfies a predicate. If not, returns the provided error.
    /// </summary>
    public static async Task<Maybe<TValue>> EnsureAsync<TValue>(
        this Maybe<TValue> maybe,
        Func<TValue, Task<bool>> predicateAsync,
        Error error)
    {
        if (maybe.IsError)
        {
            return maybe.ErrorOrThrow();
        }

        return await predicateAsync(maybe.ValueOrThrow()).ConfigureAwait(false)
            ? maybe.ValueOrThrow()
            : error;
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then applies the asynchronous Ensure validation.
    /// </summary>
    public static async Task<Maybe<TValue>> EnsureAsync<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<bool>> predicateAsync,
        Error error)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.EnsureAsync(predicateAsync, error).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then applies the asynchronous Ensure validation.
    /// </summary>
    public static async Task<Maybe<TValue>> EnsureAsync<TValue>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, Task<bool>> predicateAsync,
        Error error)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.EnsureAsync(predicateAsync, error).ConfigureAwait(false);
    }

    #endregion
}

