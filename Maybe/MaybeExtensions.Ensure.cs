using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maybe;

public static partial class MaybeExtensions
{
    /// <summary>
    /// Ensures that the success value satisfies a given condition.
    /// If the condition is not met, returns the specified error.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TValue, TError> Ensure<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, bool> predicate,
        TError error)
        where TError : IError
    {
        if (maybe.IsError)
        {
            return maybe;
        }

        if (!predicate(maybe.ValueOrThrow()))
        {
            return error;
        }

        return maybe;
    }

    /// <summary>
    /// Asynchronously ensures that the success value satisfies a given condition.
    /// If the condition is not met, returns the specified error.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> Ensure<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, bool> predicate,
        TError error)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Ensure(predicate, error);
    }

    /// <summary>
    /// Asynchronously ensures that the success value satisfies a given async condition.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> EnsureAsync<TValue, TError>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, Task<bool>> predicateAsync,
        TError error)
        where TError : IError
    {
        if (maybe.IsError)
        {
            return maybe;
        }

        if (!await predicateAsync(maybe.ValueOrThrow()).ConfigureAwait(false))
        {
            return error;
        }

        return maybe;
    }

    /// <summary>
    /// Asynchronously ensures that the success value satisfies a given async condition.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> EnsureAsync<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<bool>> predicateAsync,
        TError error)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.EnsureAsync(predicateAsync, error).ConfigureAwait(false);
    }
}

