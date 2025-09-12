namespace Maybe;

/// <summary>
/// Provides extension methods for executing side-effect actions on a Maybe without altering its value.
/// </summary>
public static partial class MaybeExtensions
{
    #region IfSome (Execute action on success)

    /// <summary>
    /// Executes a synchronous action on the success value.
    /// </summary>
    public static Maybe<TValue, TError> IfSome<TValue, TError>(this in Maybe<TValue, TError> maybe, Action<TValue> action) where TError : Error
    {
        if (maybe.IsSuccess) action(maybe.ValueOrThrow());
        return maybe;
    }

    /// <summary>
    /// Executes a synchronous action on the success value.
    /// </summary>
    public static Maybe<TValue> IfSome<TValue>(this in Maybe<TValue> maybe, Action<TValue> action)
    {
        if (maybe.IsSuccess) action(maybe.ValueOrThrow());
        return maybe;
    }

    /// <summary>
    /// Executes an asynchronous action on the success value.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> IfSomeAsync<TValue, TError>(this Maybe<TValue, TError> maybe, Func<TValue, Task> actionAsync) where TError : Error
    {
        if (maybe.IsSuccess) await actionAsync(maybe.ValueOrThrow()).ConfigureAwait(false);
        return maybe;
    }

    /// <summary>
    /// Executes an asynchronous action on the success value.
    /// </summary>
    public static async Task<Maybe<TValue>> IfSomeAsync<TValue>(this Maybe<TValue> maybe, Func<TValue, Task> actionAsync)
    {
        if (maybe.IsSuccess) await actionAsync(maybe.ValueOrThrow()).ConfigureAwait(false);
        return maybe;
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and executes a synchronous action on its success value.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> IfSome<TValue, TError>(this Task<Maybe<TValue, TError>> maybeTask, Action<TValue> action) where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.IfSome(action);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and executes a synchronous action on its success value.
    /// </summary>
    public static async Task<Maybe<TValue>> IfSome<TValue>(this Task<Maybe<TValue>> maybeTask, Action<TValue> action)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.IfSome(action);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and executes an asynchronous action on its success value.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> IfSomeAsync<TValue, TError>(this Task<Maybe<TValue, TError>> maybeTask, Func<TValue, Task> actionAsync) where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.IfSomeAsync(actionAsync).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and executes an asynchronous action on its success value.
    /// </summary>
    public static async Task<Maybe<TValue>> IfSomeAsync<TValue>(this Task<Maybe<TValue>> maybeTask, Func<TValue, Task> actionAsync)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.IfSomeAsync(actionAsync).ConfigureAwait(false);
    }

    #endregion

    #region IfNone (Execute action on error)

    /// <summary>
    /// Executes a synchronous action on the error value.
    /// </summary>
    public static Maybe<TValue, TError> IfNone<TValue, TError>(this in Maybe<TValue, TError> maybe, Action<TError> action) where TError : Error
    {
        if (maybe.IsError) action(maybe.ErrorOrThrow());
        return maybe;
    }

    /// <summary>
    /// Executes a synchronous action on the error value.
    /// </summary>
    public static Maybe<TValue> IfNone<TValue>(this in Maybe<TValue> maybe, Action<Error> action)
    {
        if (maybe.IsError) action(maybe.ErrorOrThrow());
        return maybe;
    }

    /// <summary>
    /// Executes an asynchronous action on the error value.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> IfNoneAsync<TValue, TError>(this Maybe<TValue, TError> maybe, Func<TError, Task> actionAsync) where TError : Error
    {
        if (maybe.IsError) await actionAsync(maybe.ErrorOrThrow()).ConfigureAwait(false);
        return maybe;
    }

    /// <summary>
    /// Executes an asynchronous action on the error value.
    /// </summary>
    public static async Task<Maybe<TValue>> IfNoneAsync<TValue>(this Maybe<TValue> maybe, Func<Error, Task> actionAsync)
    {
        if (maybe.IsError) await actionAsync(maybe.ErrorOrThrow()).ConfigureAwait(false);
        return maybe;
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and executes a synchronous action on its error value.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> IfNone<TValue, TError>(this Task<Maybe<TValue, TError>> maybeTask, Action<TError> action) where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.IfNone(action);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and executes a synchronous action on its error value.
    /// </summary>
    public static async Task<Maybe<TValue>> IfNone<TValue>(this Task<Maybe<TValue>> maybeTask, Action<Error> action)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.IfNone(action);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and executes an asynchronous action on its error value.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> IfNoneAsync<TValue, TError>(this Task<Maybe<TValue, TError>> maybeTask, Func<TError, Task> actionAsync) where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.IfNoneAsync(actionAsync).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and executes an asynchronous action on its error value.
    /// </summary>
    public static async Task<Maybe<TValue>> IfNoneAsync<TValue>(this Task<Maybe<TValue>> maybeTask, Func<Error, Task> actionAsync)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.IfNoneAsync(actionAsync).ConfigureAwait(false);
    }

    #endregion
}
