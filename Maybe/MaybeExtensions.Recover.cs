namespace Maybe;

/// <summary>
/// Provides extension methods for recovering from an error state in a Maybe.
/// </summary>
public static partial class MaybeExtensions
{
    #region Source: Maybe<T, E1> (Sync)

    /// <summary>
    /// Sync -> Sync | Recovers from E1 with a new Maybe possibly containing E2.
    /// </summary>
    public static Maybe<TValue, TNewError> Recover<TValue, TError, TNewError>(
        this in Maybe<TValue, TError> maybe,
        Func<TError, Maybe<TValue, TNewError>> recoveryFunc)
        where TError : Error where TNewError : Error
    {
        return maybe.IsSuccess
            ? Maybe<TValue, TNewError>.Some(maybe.ValueOrThrow())
            : recoveryFunc(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Sync -> Sync | Recovers from E1 with a new Maybe containing the default Error type.
    /// </summary>
    public static Maybe<TValue> Recover<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Func<TError, Maybe<TValue>> recoveryFunc)
        where TError : Error
    {
        return maybe.IsSuccess
            ? Maybe<TValue>.Some(maybe.ValueOrThrow())
            : recoveryFunc(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Sync -> Async | Recovers from E1 with a new async Maybe possibly containing E2.
    /// </summary>
    public static Task<Maybe<TValue, TNewError>> RecoverAsync<TValue, TError, TNewError>(
        this Maybe<TValue, TError> maybe,
        Func<TError, Task<Maybe<TValue, TNewError>>> recoveryFuncAsync)
        where TError : Error where TNewError : Error
    {
        return maybe.IsSuccess
            ? Task.FromResult(Maybe<TValue, TNewError>.Some(maybe.ValueOrThrow()))
            : recoveryFuncAsync(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Sync -> Async | Recovers from E1 with a new async Maybe containing the default Error type.
    /// </summary>
    public static Task<Maybe<TValue>> RecoverAsync<TValue, TError>(
        this Maybe<TValue, TError> maybe,
        Func<TError, Task<Maybe<TValue>>> recoveryFuncAsync)
        where TError : Error
    {
        return maybe.IsSuccess
            ? Task.FromResult(Maybe<TValue>.Some(maybe.ValueOrThrow()))
            : recoveryFuncAsync(maybe.ErrorOrThrow());
    }

    #endregion

    #region Source: Maybe<T> (Sync)

    /// <summary>
    /// Sync -> Sync | Recovers from default Error with a new Maybe containing a specific Error E2.
    /// </summary>
    public static Maybe<TValue, TNewError> Recover<TValue, TNewError>(
        this in Maybe<TValue> maybe,
        Func<Error, Maybe<TValue, TNewError>> recoveryFunc)
        where TNewError : Error
    {
        return maybe.IsSuccess
            ? Maybe<TValue, TNewError>.Some(maybe.ValueOrThrow())
            : recoveryFunc(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Sync -> Sync | Recovers from default Error with a new Maybe containing the default Error type.
    /// </summary>
    public static Maybe<TValue> Recover<TValue>(
        this in Maybe<TValue> maybe,
        Func<Error, Maybe<TValue>> recoveryFunc)
    {
        return maybe.IsSuccess ? maybe : recoveryFunc(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Sync -> Async | Recovers from default Error with a new async Maybe containing a specific Error E2.
    /// </summary>
    public static Task<Maybe<TValue, TNewError>> RecoverAsync<TValue, TNewError>(
        this Maybe<TValue> maybe,
        Func<Error, Task<Maybe<TValue, TNewError>>> recoveryFuncAsync)
        where TNewError : Error
    {
        return maybe.IsSuccess
            ? Task.FromResult(Maybe<TValue, TNewError>.Some(maybe.ValueOrThrow()))
            : recoveryFuncAsync(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Sync -> Async | Recovers from default Error with a new async Maybe containing the default Error type.
    /// </summary>
    public static Task<Maybe<TValue>> RecoverAsync<TValue>(
        this Maybe<TValue> maybe,
        Func<Error, Task<Maybe<TValue>>> recoveryFuncAsync)
    {
        return maybe.IsSuccess
            ? Task.FromResult(maybe)
            : recoveryFuncAsync(maybe.ErrorOrThrow());
    }

    #endregion

    #region Source: Task<Maybe<T, E1>> (Async)

    /// <summary>
    /// Async -> Sync | Recovers from E1 with a new Maybe possibly containing E2.
    /// </summary>
    public static async Task<Maybe<TValue, TNewError>> Recover<TValue, TError, TNewError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, Maybe<TValue, TNewError>> recoveryFunc)
        where TError : Error where TNewError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Recover(recoveryFunc);
    }

    /// <summary>
    /// Async -> Sync | Recovers from E1 with a new Maybe containing the default Error type.
    /// </summary>
    public static async Task<Maybe<TValue>> Recover<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, Maybe<TValue>> recoveryFunc)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Recover(recoveryFunc);
    }

    /// <summary>
    /// Async -> Async | Recovers from E1 with a new async Maybe possibly containing E2.
    /// </summary>
    public static async Task<Maybe<TValue, TNewError>> RecoverAsync<TValue, TError, TNewError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, Task<Maybe<TValue, TNewError>>> recoveryFuncAsync)
        where TError : Error where TNewError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.RecoverAsync(recoveryFuncAsync).ConfigureAwait(false);
    }

    /// <summary>
    /// Async -> Async | Recovers from E1 with a new async Maybe containing the default Error type.
    /// </summary>
    public static async Task<Maybe<TValue>> RecoverAsync<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, Task<Maybe<TValue>>> recoveryFuncAsync)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.RecoverAsync(recoveryFuncAsync).ConfigureAwait(false);
    }

    #endregion

    #region Source: Task<Maybe<T>> (Async)

    /// <summary>
    /// Async -> Sync | Recovers from default Error with a new Maybe containing a specific Error E2.
    /// </summary>
    public static async Task<Maybe<TValue, TNewError>> Recover<TValue, TNewError>(
        this Task<Maybe<TValue>> maybeTask,
        Func<Error, Maybe<TValue, TNewError>> recoveryFunc)
        where TNewError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Recover(recoveryFunc);
    }

    /// <summary>
    /// Async -> Sync | Recovers from default Error with a new Maybe containing the default Error type.
    /// </summary>
    public static async Task<Maybe<TValue>> Recover<TValue>(
        this Task<Maybe<TValue>> maybeTask,
        Func<Error, Maybe<TValue>> recoveryFunc)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Recover(recoveryFunc);
    }

    /// <summary>
    /// Async -> Async | Recovers from default Error with a new async Maybe containing a specific Error E2.
    /// </summary>
    public static async Task<Maybe<TValue, TNewError>> RecoverAsync<TValue, TNewError>(
        this Task<Maybe<TValue>> maybeTask,
        Func<Error, Task<Maybe<TValue, TNewError>>> recoveryFuncAsync)
        where TNewError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.RecoverAsync(recoveryFuncAsync).ConfigureAwait(false);
    }

    /// <summary>
    /// Async -> Async | Recovers from default Error with a new async Maybe containing the default Error type.
    /// </summary>
    public static async Task<Maybe<TValue>> RecoverAsync<TValue>(
        this Task<Maybe<TValue>> maybeTask,
        Func<Error, Task<Maybe<TValue>>> recoveryFuncAsync)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.RecoverAsync(recoveryFuncAsync).ConfigureAwait(false);
    }

    #endregion
}

