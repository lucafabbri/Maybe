using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maybe;

public static partial class MaybeExtensions
{
    /// <summary>
    /// Recovers from an error state by applying a recovery function that returns a new Maybe.
    /// Allows for fallback logic.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TValue, TError> Recover<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Func<TError, Maybe<TValue, TError>> recoveryFunc)
        where TError : IError
    {
        return maybe.IsError
            ? recoveryFunc(maybe.ErrorOrThrow())
            : maybe;
    }

    /// <summary>
    /// Asynchronously recovers from an error state by applying a recovery function.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> Recover<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, Maybe<TValue, TError>> recoveryFunc)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Recover(recoveryFunc);
    }

    /// <summary>
    /// Asynchronously recovers from an error state by applying an async recovery function.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> RecoverAsync<TValue, TError>(
        this Maybe<TValue, TError> maybe,
        Func<TError, Task<Maybe<TValue, TError>>> recoveryFuncAsync)
        where TError : IError
    {
        return maybe.IsError
            ? await recoveryFuncAsync(maybe.ErrorOrThrow()).ConfigureAwait(false)
            : maybe;
    }

    /// <summary>
    /// Asynchronously recovers from an error state by applying an async recovery function.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> RecoverAsync<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, Task<Maybe<TValue, TError>>> recoveryFuncAsync)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.RecoverAsync(recoveryFuncAsync).ConfigureAwait(false);
    }
}

