namespace Maybe;

/// <summary>
/// Provides terminal extension methods for executing side-effect actions on a Maybe.
/// Unlike IfSome/IfNone, these methods do not return the Maybe and terminate the chain.
/// </summary>
public static partial class MaybeExtensions
{
    #region ThenDo (Execute terminal action on success)

    /// <summary>
    /// If the outcome is a success, executes a synchronous action on the value and terminates the chain.
    /// </summary>
    public static void ThenDo<TValue, TError>(this in Maybe<TValue, TError> maybe, Action<TValue> action) where TError : Error
    {
        if (maybe.IsSuccess)
        {
            action(maybe.ValueOrThrow());
        }
    }

    /// <summary>
    /// If the outcome is a success, executes an asynchronous action on the value and terminates the chain.
    /// </summary>
    public static Task ThenDoAsync<TValue, TError>(this Maybe<TValue, TError> maybe, Func<TValue, Task> actionAsync) where TError : Error
    {
        return maybe.IsSuccess ? actionAsync(maybe.ValueOrThrow()) : Task.CompletedTask;
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and, if it is a success, executes a synchronous action on the value.
    /// </summary>
    public static async Task ThenDo<TValue, TError>(this Task<Maybe<TValue, TError>> maybeTask, Action<TValue> action) where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        maybe.ThenDo(action);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and, if it is a success, executes an asynchronous action on the value.
    /// </summary>
    public static async Task ThenDoAsync<TValue, TError>(this Task<Maybe<TValue, TError>> maybeTask, Func<TValue, Task> actionAsync) where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        await maybe.ThenDoAsync(actionAsync).ConfigureAwait(false);
    }

    #endregion

    #region ElseDo (Execute terminal action on error)

    /// <summary>
    /// If the outcome is an error, executes a synchronous action on the error and terminates the chain.
    /// </summary>
    public static void ElseDo<TValue, TError>(this in Maybe<TValue, TError> maybe, Action<TError> action) where TError : Error
    {
        if (maybe.IsError)
        {
            action(maybe.ErrorOrThrow());
        }
    }

    /// <summary>
    /// If the outcome is an error, executes an asynchronous action on the error and terminates the chain.
    /// </summary>
    public static Task ElseDoAsync<TValue, TError>(this Maybe<TValue, TError> maybe, Func<TError, Task> actionAsync) where TError : Error
    {
        return maybe.IsError ? actionAsync(maybe.ErrorOrThrow()) : Task.CompletedTask;
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and, if it is an error, executes a synchronous action on the error.
    /// </summary>
    public static async Task ElseDo<TValue, TError>(this Task<Maybe<TValue, TError>> maybeTask, Action<TError> action) where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        maybe.ElseDo(action);
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and, if it is an error, executes an asynchronous action on the error.
    /// </summary>
    public static async Task ElseDoAsync<TValue, TError>(this Task<Maybe<TValue, TError>> maybeTask, Func<TError, Task> actionAsync) where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        await maybe.ElseDoAsync(actionAsync).ConfigureAwait(false);
    }

    #endregion
}
