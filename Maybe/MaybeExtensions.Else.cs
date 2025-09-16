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
    public static Maybe<TValue, TError> Else<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        TValue fallbackValue)
        where TError : BaseError, new()
    {
        return maybe.IsSuccess
            ? maybe.ValueOrThrow()
            : fallbackValue;
    }

    public static Maybe<TValue, TError> Else<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        TError forwardedError)
        where TError : BaseError, new()
    {
        return maybe.IsSuccess
            ? maybe.ValueOrThrow()
            : forwardedError;
    }

    /// <summary>
    /// Returns the success value or, if the Maybe is an error, the result of the provided fallback function.
    /// </summary>
    public static Maybe<TValue, TError> Else<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Func<TError, TValue> fallbackFunc)
        where TError : BaseError, new()
    {
        return maybe.IsSuccess
            ? maybe.ValueOrThrow()
            : fallbackFunc(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Returns the success value or, if the Maybe is an error, the provided fallback error.
    /// </summary>
    public static Maybe<TValue, TError> Else<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Func<TError, TError> fallbackErrorFunc)
        where TError : BaseError, new()
    {
        return maybe.IsSuccess
            ? maybe.ValueOrThrow()
            : fallbackErrorFunc(maybe.ErrorOrThrow());
    }

    #endregion

    #region Asynchronous Else on Task<Maybe> (with Sync Fallback)

    /// <summary>
    /// Asynchronously returns the success value or, if the Maybe is an error, the provided fallback value.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> Else<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        TValue fallbackValue)
        where TError : BaseError, new()
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Else(fallbackValue);
    }


    public static async Task<Maybe<TValue, TError>> Else<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        TError forwaredError)
        where TError : BaseError, new()
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Else(forwaredError);
    }

    /// <summary>
    /// Asynchronously returns the success value or, if the Maybe is an error, the result of the provided fallback function.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> Else<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, TValue> fallbackFunc)
        where TError : BaseError, new()
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Else(fallbackFunc);
    }


    public static async Task<Maybe<TValue, TError>> Else<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, TError> fallbackErrorFunc)
        where TError : BaseError, new()
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Else(fallbackErrorFunc);
    }

    #endregion

    #region Asynchronous ElseAsync (with Async Fallback)

    /// <summary>
    /// Returns the success value or, if the Maybe is an error, asynchronously executes the fallback function and returns its result.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> ElseAsync<TValue, TError>(
        this Maybe<TValue, TError> maybe,
        Func<TError, Task<TValue>> fallbackAsync)
        where TError : BaseError, new()
    {
        return maybe.IsSuccess
            ? maybe
            : await fallbackAsync(maybe.ErrorOrThrow());
    }

    public static async Task<Maybe<TValue, TError>> ElseAsync<TValue, TError>(
        this Maybe<TValue, TError> maybe,
        Func<TError, Task<TError>> forwardedErrorAsync)
        where TError : BaseError, new()
    {
        return maybe.IsSuccess
            ? maybe
            : await forwardedErrorAsync(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Asynchronously returns the success value or, if the Maybe is an error, asynchronously executes the fallback function and returns its result.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> ElseAsync<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, Task<TValue>> fallbackAsync)
        where TError : BaseError, new()
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.ElseAsync(fallbackAsync).ConfigureAwait(false);
    }


    public static async Task<Maybe<TValue, TError>> ElseAsync<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, Task<TError>> fallbackErrorAsync)
        where TError : BaseError, new()
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.ElseAsync(fallbackErrorAsync).ConfigureAwait(false);
    }

    #endregion
}
