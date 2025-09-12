namespace Maybe;

/// <summary>
/// Provides extension methods for transforming a Maybe by handling both its success and error states.
/// These are terminal operations that unwrap the Maybe into a different type.
/// </summary>
public static partial class MaybeExtensions
{
    #region Synchronous Match (Sync Functions)

    /// <summary>
    /// Transforms the Maybe by applying one of two synchronous functions, depending on its state.
    /// </summary>
    public static TResult Match<TValue, TError, TResult>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, TResult> onSome,
        Func<TError, TResult> onNone)
        where TError : Error
    {
        return maybe.IsSuccess
            ? onSome(maybe.ValueOrThrow())
            : onNone(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Transforms the Maybe by applying one of two synchronous functions, depending on its state.
    /// </summary>
    public static TResult Match<TValue, TResult>(
        this in Maybe<TValue> maybe,
        Func<TValue, TResult> onSome,
        Func<Error, TResult> onNone)
    {
        return maybe.IsSuccess
            ? onSome(maybe.ValueOrThrow())
            : onNone(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then transforms it by applying one of two synchronous functions.
    /// </summary>
    public static async Task<TResult> Match<TValue, TError, TResult>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, TResult> onSome,
        Func<TError, TResult> onNone)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Match(onSome, onNone);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then transforms it by applying one of two synchronous functions.
    /// </summary>
    public static async Task<TResult> Match<TValue, TResult>(
        this Task<Maybe<TValue>> maybetask,
        Func<TValue, TResult> onSome,
        Func<Error, TResult> onNone)
    {
        var maybe = await maybetask.ConfigureAwait(false);
        return maybe.Match(onSome, onNone);
    }

    #endregion

    #region Asynchronous Match (Async Functions)

    /// <summary>
    /// Asynchronously transforms the Maybe by applying one of two asynchronous functions, depending on its state.
    /// </summary>
    public static Task<TResult> MatchAsync<TValue, TError, TResult>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, Task<TResult>> onSomeAsync,
        Func<TError, Task<TResult>> onNoneAsync)
        where TError : Error
    {
        return maybe.IsSuccess
            ? onSomeAsync(maybe.ValueOrThrow())
            : onNoneAsync(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Asynchronously transforms the Maybe by applying a mix of synchronous and asynchronous functions.
    /// </summary>
    public static async Task<TResult> MatchAsync<TValue, TError, TResult>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, Task<TResult>> onSomeAsync,
        Func<TError, TResult> onNone)
        where TError : Error
    {
        return maybe.IsSuccess
            ? await onSomeAsync(maybe.ValueOrThrow()).ConfigureAwait(false)
            : onNone(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Asynchronously transforms the Maybe by applying a mix of synchronous and asynchronous functions.
    /// </summary>
    public static async Task<TResult> MatchAsync<TValue, TError, TResult>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, TResult> onSome,
        Func<TError, Task<TResult>> onNoneAsync)
        where TError : Error
    {
        return maybe.IsSuccess
            ? onSome(maybe.ValueOrThrow())
            : await onNoneAsync(maybe.ErrorOrThrow()).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then transforms it by applying one of two asynchronous functions.
    /// </summary>
    public static async Task<TResult> MatchAsync<TValue, TError, TResult>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<TResult>> onSomeAsync,
        Func<TError, Task<TResult>> onNoneAsync)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.MatchAsync(onSomeAsync, onNoneAsync).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then transforms it by applying a mix of synchronous and asynchronous functions.
    /// </summary>
    public static async Task<TResult> MatchAsync<TValue, TError, TResult>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<TResult>> onSomeAsync,
        Func<TError, TResult> onNone)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.MatchAsync(onSomeAsync, onNone).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then transforms it by applying a mix of synchronous and asynchronous functions.
    /// </summary>
    public static async Task<TResult> MatchAsync<TValue, TError, TResult>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, TResult> onSome,
        Func<TError, Task<TResult>> onNoneAsync)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.MatchAsync(onSome, onNoneAsync).ConfigureAwait(false);
    }

    // Overloads for Maybe<TValue>
    public static Task<TResult> MatchAsync<TValue, TResult>(this Maybe<TValue> maybe, Func<TValue, Task<TResult>> onSomeAsync, Func<Error, Task<TResult>> onNoneAsync)
        => maybe.IsSuccess ? onSomeAsync(maybe.ValueOrThrow()) : onNoneAsync(maybe.ErrorOrThrow());

    public static async Task<TResult> MatchAsync<TValue, TResult>(this Maybe<TValue> maybe, Func<TValue, Task<TResult>> onSomeAsync, Func<Error, TResult> onNone)
        => maybe.IsSuccess ? await onSomeAsync(maybe.ValueOrThrow()).ConfigureAwait(false) : onNone(maybe.ErrorOrThrow());

    public static async Task<TResult> MatchAsync<TValue, TResult>(this Maybe<TValue> maybe, Func<TValue, TResult> onSome, Func<Error, Task<TResult>> onNoneAsync)
        => maybe.IsSuccess ? onSome(maybe.ValueOrThrow()) : await onNoneAsync(maybe.ErrorOrThrow()).ConfigureAwait(false);

    public static async Task<TResult> MatchAsync<TValue, TResult>(this Task<Maybe<TValue>> maybeTask, Func<TValue, Task<TResult>> onSomeAsync, Func<Error, Task<TResult>> onNoneAsync)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.MatchAsync(onSomeAsync, onNoneAsync).ConfigureAwait(false);
    }

    public static async Task<TResult> MatchAsync<TValue, TResult>(this Task<Maybe<TValue>> maybeTask, Func<TValue, Task<TResult>> onSomeAsync, Func<Error, TResult> onNone)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.MatchAsync(onSomeAsync, onNone).ConfigureAwait(false);
    }

    public static async Task<TResult> MatchAsync<TValue, TResult>(this Task<Maybe<TValue>> maybeTask, Func<TValue, TResult> onSome, Func<Error, Task<TResult>> onNoneAsync)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.MatchAsync(onSome, onNoneAsync).ConfigureAwait(false);
    }

    #endregion
}
