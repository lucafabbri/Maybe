namespace Maybe;

/// <summary>
/// Provides extension methods for exiting the Maybe context by providing a fallback value or function.
/// These are terminal operations that unwrap the Maybe.
/// </summary>
public static partial class MaybeExtensions
{
    #region Synchronous Else (with Sync Fallback)

    /// <summary>
    /// Returns the original success as-is; otherwise returns the provided <paramref name="fallbackValue"/>.
    /// </summary>
    /// <remarks>
    /// Note: passing null as <paramref name="fallbackValue"/> may be ambiguous when both TValue and TError
    /// are reference types and other overloads exist. Prefer explicit casts or the delegate-based overloads.
    /// </remarks>
    /// <param name="maybe">The Maybe to evaluate.</param>
    /// <param name="fallbackValue">The value to return if <paramref name="maybe"/> is an error.</param>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TError">The error type.</typeparam>
    /// <returns>
    /// A successful Maybe with the original value, or a successful Maybe with <paramref name="fallbackValue"/>.
    /// </returns>
    public static Maybe<TValue, TError> Else<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        TValue fallbackValue)
        where TError : BaseError, new()
    {
        return maybe.IsSuccess
            ? maybe
            : fallbackValue;
    }

    /// <summary>
    /// Returns the original success as-is; otherwise replaces the original error with the provided <paramref name="forwardedError"/>.
    /// </summary>
    /// <remarks>
    /// Useful to adapt low-level errors to higher-level domain errors. The original error is discarded.
    /// </remarks>
    /// <param name="maybe">The Maybe to evaluate.</param>
    /// <param name="forwardedError">The error to return if <paramref name="maybe"/> is an error.</param>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TError">The error type.</typeparam>
    /// <returns>
    /// A successful Maybe with the original value, or an error Maybe with <paramref name="forwardedError"/>.
    /// </returns>
    public static Maybe<TValue, TError> Else<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        TError forwardedError)
        where TError : BaseError, new()
    {
        return maybe.IsSuccess
            ? maybe
            : forwardedError;
    }

    /// <summary>
    /// Returns the original success as-is; otherwise returns the result of <paramref name="fallbackFunc"/> invoked with the original error.
    /// </summary>
    /// <remarks>
    /// The function is not invoked if the Maybe is successful.
    /// </remarks>
    /// <param name="maybe">The Maybe to evaluate.</param>
    /// <param name="fallbackFunc">The function that maps the original error to a fallback success value.</param>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TError">The error type.</typeparam>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fallbackFunc"/> is null.</exception>
    public static Maybe<TValue, TError> Else<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Func<TError, TValue> fallbackFunc)
        where TError : BaseError, new()
    {
        if (fallbackFunc is null) throw new ArgumentNullException(nameof(fallbackFunc));

        return maybe.IsSuccess
            ? maybe
            : fallbackFunc(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Returns the original success as-is; otherwise transforms the original error via <paramref name="fallbackErrorFunc"/>.
    /// </summary>
    /// <remarks>
    /// The function is not invoked if the Maybe is successful.
    /// </remarks>
    /// <param name="maybe">The Maybe to evaluate.</param>
    /// <param name="fallbackErrorFunc">The function that maps the original error to a new error to return.</param>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TError">The error type.</typeparam>
    /// <returns>
    /// A successful Maybe with the original value, or an error Maybe with the transformed error.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fallbackErrorFunc"/> is null.</exception>
    public static Maybe<TValue, TError> Else<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Func<TError, TError> fallbackErrorFunc)
        where TError : BaseError, new()
    {
        if (fallbackErrorFunc is null) throw new ArgumentNullException(nameof(fallbackErrorFunc));

        return maybe.IsSuccess
            ? maybe
            : fallbackErrorFunc(maybe.ErrorOrThrow());
    }

    #endregion

    #region Asynchronous Else on Task<Maybe> (with Sync Fallback)

    /// <summary>
    /// Asynchronously returns the original success as-is; otherwise returns the provided <paramref name="fallbackValue"/>.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> Else<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        TValue fallbackValue)
        where TError : BaseError, new()
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Else(fallbackValue);
    }

    /// <summary>
    /// Asynchronously returns the original success as-is; otherwise replaces the original error with <paramref name="forwardedError"/>.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> Else<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        TError forwardedError)
        where TError : BaseError, new()
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Else(forwardedError);
    }

    /// <summary>
    /// Asynchronously returns the original success as-is; otherwise returns the result of <paramref name="fallbackFunc"/> invoked with the original error.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fallbackFunc"/> is null.</exception>
    public static async Task<Maybe<TValue, TError>> Else<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, TValue> fallbackFunc)
        where TError : BaseError, new()
    {
        if (fallbackFunc is null) throw new ArgumentNullException(nameof(fallbackFunc));

        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Else(fallbackFunc);
    }

    /// <summary>
    /// Asynchronously returns the original success as-is; otherwise transforms the original error via <paramref name="fallbackErrorFunc"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fallbackErrorFunc"/> is null.</exception>
    public static async Task<Maybe<TValue, TError>> Else<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, TError> fallbackErrorFunc)
        where TError : BaseError, new()
    {
        if (fallbackErrorFunc is null) throw new ArgumentNullException(nameof(fallbackErrorFunc));

        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Else(fallbackErrorFunc);
    }

    #endregion

    #region Asynchronous ElseAsync (with Async Fallback)

    /// <summary>
    /// Returns the original success as-is; otherwise asynchronously executes <paramref name="fallbackAsync"/> and returns its result.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fallbackAsync"/> is null.</exception>
    public static async Task<Maybe<TValue, TError>> ElseAsync<TValue, TError>(
        this Maybe<TValue, TError> maybe,
        Func<TError, Task<TValue>> fallbackAsync)
        where TError : BaseError, new()
    {
        if (fallbackAsync is null) throw new ArgumentNullException(nameof(fallbackAsync));

        return maybe.IsSuccess
            ? maybe
            : await fallbackAsync(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Returns the original success as-is; otherwise asynchronously transforms the original error via <paramref name="forwardedErrorAsync"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="forwardedErrorAsync"/> is null.</exception>
    public static async Task<Maybe<TValue, TError>> ElseAsync<TValue, TError>(
        this Maybe<TValue, TError> maybe,
        Func<TError, Task<TError>> forwardedErrorAsync)
        where TError : BaseError, new()
    {
        if (forwardedErrorAsync is null) throw new ArgumentNullException(nameof(forwardedErrorAsync));

        return maybe.IsSuccess
            ? maybe
            : await forwardedErrorAsync(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Asynchronously returns the original success as-is; otherwise asynchronously executes <paramref name="fallbackAsync"/> and returns its result.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fallbackAsync"/> is null.</exception>
    public static async Task<Maybe<TValue, TError>> ElseAsync<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, Task<TValue>> fallbackAsync)
        where TError : BaseError, new()
    {
        if (fallbackAsync is null) throw new ArgumentNullException(nameof(fallbackAsync));

        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.ElseAsync(fallbackAsync).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously returns the original success as-is; otherwise asynchronously transforms the original error via <paramref name="fallbackErrorAsync"/>.
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="fallbackErrorAsync"/> is null.</exception>
    public static async Task<Maybe<TValue, TError>> ElseAsync<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, Task<TError>> fallbackErrorAsync)
        where TError : BaseError, new()
    {
        if (fallbackErrorAsync is null) throw new ArgumentNullException(nameof(fallbackErrorAsync));

        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.ElseAsync(fallbackErrorAsync).ConfigureAwait(false);
    }

    #endregion
}