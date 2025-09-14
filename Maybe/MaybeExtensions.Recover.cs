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
        where TError : Error, new() 
        where TNewError : Error, new()
    {
        return maybe.IsSuccess
            ? Maybe<TValue, TNewError>.Some(maybe.ValueOrThrow())
            : recoveryFunc(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Sync -> Async | Recovers from E1 with a new async Maybe possibly containing E2.
    /// </summary>
    public static Task<Maybe<TValue, TNewError>> RecoverAsync<TValue, TError, TNewError>(
        this Maybe<TValue, TError> maybe,
        Func<TError, Task<Maybe<TValue, TNewError>>> recoveryFuncAsync)
        where TError : Error, new() 
        where TNewError : Error, new()
    {
        return maybe.IsSuccess
            ? Task.FromResult(Maybe<TValue, TNewError>.Some(maybe.ValueOrThrow()))
            : recoveryFuncAsync(maybe.ErrorOrThrow());
    }

    #endregion

    #region Source: Maybe<T> (Sync)

    #endregion

    #region Source: Task<Maybe<T, E1>> (Async)

    /// <summary>
    /// Async -> Sync | Recovers from E1 with a new Maybe possibly containing E2.
    /// </summary>
    public static async Task<Maybe<TValue, TNewError>> Recover<TValue, TError, TNewError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, Maybe<TValue, TNewError>> recoveryFunc)
        where TError : Error, new() where TNewError : Error, new()
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
        where TError : Error, new() where TNewError : Error, new()
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.RecoverAsync(recoveryFuncAsync).ConfigureAwait(false);
    }

    #endregion
}

