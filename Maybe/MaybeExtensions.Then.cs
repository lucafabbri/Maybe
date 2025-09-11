using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maybe;

public static partial class MaybeExtensions
{
    /// <summary>
    /// If the Maybe is a success, applies the provided function, flattening the result.
    /// This is the primary method for chaining operations that return a Maybe.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TNewValue, TError> Then<TValue, TError, TNewValue>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, Maybe<TNewValue, TError>> func)
        where TError : IError
    {
        return maybe.IsSuccess
            ? func(maybe.ValueOrThrow())
            : Maybe<TNewValue, TError>.None(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Enables LINQ query syntax for chaining multiple Maybe-returning operations.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TResult, TError> SelectMany<TValue, TError, TIntermediate, TResult>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, Maybe<TIntermediate, TError>> intermediateSelector,
        Func<TValue, TIntermediate, TResult> resultSelector)
        where TError : IError
    {
        if (maybe.IsError)
        {
            return Maybe<TResult, TError>.None(maybe.ErrorOrThrow());
        }

        var intermediateMaybe = intermediateSelector(maybe.ValueOrThrow());

        if (intermediateMaybe.IsError)
        {
            return Maybe<TResult, TError>.None(intermediateMaybe.ErrorOrThrow());
        }

        return Maybe<TResult, TError>.Some(resultSelector(maybe.ValueOrThrow(), intermediateMaybe.ValueOrThrow()));
    }

    /// <summary>
    /// If the awaited Maybe is a success, applies the provided synchronous function.
    /// </summary>
    public static async Task<Maybe<TNewValue, TError>> Then<TValue, TError, TNewValue>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Maybe<TNewValue, TError>> func)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Then(func);
    }

    /// <summary>
    /// If the Maybe is a success, applies the provided asynchronous function.
    /// </summary>
    public static async Task<Maybe<TNewValue, TError>> ThenAsync<TValue, TError, TNewValue>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, Task<Maybe<TNewValue, TError>>> funcAsync)
        where TError : IError
    {
        return maybe.IsSuccess
            ? await funcAsync(maybe.ValueOrThrow()).ConfigureAwait(false)
            // Note: Explicitly creating the Task to match the return type.
            : Maybe<TNewValue, TError>.None(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// If the awaited Maybe is a success, applies the provided asynchronous function.
    /// </summary>
    public static async Task<Maybe<TNewValue, TError>> ThenAsync<TValue, TError, TNewValue>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<Maybe<TNewValue, TError>>> funcAsync)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.ThenAsync(funcAsync).ConfigureAwait(false);
    }
}
