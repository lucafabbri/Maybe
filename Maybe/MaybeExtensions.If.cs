using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maybe;

public static partial class MaybeExtensions
{
    /// <summary>
    /// Executes an action on the success value if it exists, without altering the Maybe.
    /// Useful for side effects like logging.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TValue, TError> IfSome<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Action<TValue> action)
        where TError : IError
    {
        if (maybe.IsSuccess)
        {
            action(maybe.ValueOrThrow());
        }

        return maybe;
    }

    /// <summary>
    /// Executes an action on the error if it exists, without altering the Maybe.
    /// Useful for side effects like logging.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Maybe<TValue, TError> IfNone<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Action<TError> action)
        where TError : IError
    {
        if (maybe.IsError)
        {
            action(maybe.ErrorOrThrow());
        }

        return maybe;
    }

    /// <summary>
    /// Asynchronously executes an action on the success value if it exists, without altering the Maybe.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> IfSome<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Action<TValue> action)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.IfSome(action);
    }

    /// <summary>
    /// Asynchronously executes an action on the error if it exists, without altering the Maybe.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> IfNone<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Action<TError> action)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.IfNone(action);
    }

    /// <summary>
    /// Asynchronously executes an async action on the success value if it exists, without altering the Maybe.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> IfSomeAsync<TValue, TError>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, Task> actionAsync)
        where TError : IError
    {
        if (maybe.IsSuccess)
        {
            await actionAsync(maybe.ValueOrThrow()).ConfigureAwait(false);
        }

        return maybe;
    }

    /// <summary>
    /// Asynchronously executes an async action on the error if it exists, without altering the Maybe.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> IfNoneAsync<TValue, TError>(
        this Maybe<TValue, TError> maybe,
        Func<TError, Task> actionAsync)
        where TError : IError
    {
        if (maybe.IsError)
        {
            await actionAsync(maybe.ErrorOrThrow()).ConfigureAwait(false);
        }

        return maybe;
    }

    /// <summary>
    /// Asynchronously executes an async action on the success value if it exists, without altering the Maybe.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> IfSomeAsync<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task> actionAsync)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.IfSomeAsync(actionAsync).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously executes an async action on the error if it exists, without altering the Maybe.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> IfNoneAsync<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, Task> actionAsync)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.IfNoneAsync(actionAsync).ConfigureAwait(false);
    }
}

