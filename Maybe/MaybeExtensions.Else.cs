namespace Maybe;

/// <summary>
/// Provides extension methods for exiting the Maybe context by providing a fallback value or function.
/// These are terminal operations that unwrap the Maybe.
/// </summary>
public static partial class MaybeExtensions
{
    #region Synchronous Else (with Sync Fallback)

    /// <summary>
    /// Returns the success value or, if the Maybe is an error, the provided fallback value.
    /// </summary>
    public static TValue Else<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        TValue fallbackValue)
        where TError : BaseError, new()
    {
        return maybe.IsSuccess
            ? maybe.ValueOrThrow()
            : fallbackValue;
    }

    /// <summary>
    /// Returns the success value or, if the Maybe is an error, the result of the provided fallback function.
    /// </summary>
    public static TValue Else<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Func<TError, TValue> fallbackFunc)
        where TError : BaseError, new()
    {
        return maybe.IsSuccess
            ? maybe.ValueOrThrow()
            : fallbackFunc(maybe.ErrorOrThrow());
    }

    #endregion

    #region Asynchronous Else on Task<Maybe> (with Sync Fallback)

    /// <summary>
    /// Asynchronously returns the success value or, if the Maybe is an error, the provided fallback value.
    /// </summary>
    public static async Task<TValue> Else<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        TValue fallbackValue)
        where TError : BaseError, new()
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Else(fallbackValue);
    }

    /// <summary>
    /// Asynchronously returns the success value or, if the Maybe is an error, the result of the provided fallback function.
    /// </summary>
    public static async Task<TValue> Else<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, TValue> fallbackFunc)
        where TError : BaseError, new()
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Else(fallbackFunc);
    }

    #endregion

    #region Asynchronous ElseAsync (with Async Fallback)

    /// <summary>
    /// Returns the success value or, if the Maybe is an error, asynchronously executes the fallback function and returns its result.
    /// </summary>
    public static async Task<TValue> ElseAsync<TValue, TError>(
        this Maybe<TValue, TError> maybe,
        Func<TError, Task<TValue>> fallbackAsync)
        where TError : BaseError, new()
    {
        return maybe.IsSuccess
            ? maybe.ValueOrThrow()
            : await fallbackAsync(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Asynchronously returns the success value or, if the Maybe is an error, asynchronously executes the fallback function and returns its result.
    /// </summary>
    public static async Task<TValue> ElseAsync<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, Task<TValue>> fallbackAsync)
        where TError : BaseError, new()
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.ElseAsync(fallbackAsync).ConfigureAwait(false);
    }

    #endregion
}
