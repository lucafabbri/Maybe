using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maybe;

public static partial class MaybeExtensions
{
    /// <summary>
    /// Transforms the success value of a Maybe without leaving the Maybe context.
    /// If the Maybe is an error, the error is propagated.
    /// This is an alias for Map and is used to enable LINQ query syntax.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TNewValue, TError> Select<TValue, TError, TNewValue>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, TNewValue> selector)
        where TError : IError
    {
        return maybe.IsSuccess
            ? Maybe<TNewValue, TError>.Some(selector(maybe.ValueOrThrow()))
            : Maybe<TNewValue, TError>.None(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Transforms the success value of a Maybe without leaving the Maybe context.
    /// If the Maybe is an error, the error is propagated.
    /// This is an alias for Select.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TNewValue, TError> Map<TValue, TError, TNewValue>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, TNewValue> selector)
        where TError : IError =>
        maybe.Select(selector);

    /// <summary>
    /// Asynchronously transforms the success value of a Maybe without leaving the Maybe context.
    /// </summary>
    public static async Task<Maybe<TNewValue, TError>> Select<TValue, TError, TNewValue>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, TNewValue> selector)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Select(selector);
    }

    /// <summary>
    /// Asynchronously transforms the success value of a Maybe using an async selector function.
    /// </summary>
    public static async Task<Maybe<TNewValue, TError>> SelectAsync<TValue, TError, TNewValue>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, Task<TNewValue>> selectorAsync)
        where TError : IError
    {
        if (maybe.IsError)
        {
            return Maybe<TNewValue, TError>.None(maybe.ErrorOrThrow());
        }

        var newValue = await selectorAsync(maybe.ValueOrThrow()).ConfigureAwait(false);
        return Maybe<TNewValue, TError>.Some(newValue);
    }

    /// <summary>
    /// Asynchronously transforms the success value of a Maybe contained in a Task using an async selector function.
    /// </summary>
    public static async Task<Maybe<TNewValue, TError>> SelectAsync<TValue, TError, TNewValue>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<TNewValue>> selectorAsync)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.SelectAsync(selectorAsync).ConfigureAwait(false);
    }
}

