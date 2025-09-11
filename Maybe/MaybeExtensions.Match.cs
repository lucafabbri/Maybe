namespace Maybe;

public static partial class MaybeExtensions
{
    /// <summary>
    /// Transforms the Maybe by applying one of two functions, depending on the outcome's state.
    /// This forces the handling of both success and error states.
    /// </summary>
    public static TResult Match<TValue, TError, TResult>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, TResult> onSome,
        Func<TError, TResult> onNone)
        where TError : IError
    {
        return maybe.IsSuccess
            ? onSome(maybe.ValueOrThrow())
            : onNone(maybe.ErrorOrThrow());
    }

    public static async Task<TResult> Match<TValue, TError, TResult>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, TResult> onSome,
        Func<TError, TResult> onNone)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.IsSuccess
            ? onSome(maybe.ValueOrThrow())
            : onNone(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Transforms the Maybe by applying one of two functions, depending on the outcome's state.
    /// This forces the handling of both success and error states.
    /// </summary>
    public static TResult Match<TValue, TResult>(
        this in Maybe<TValue> maybe,
        Func<TValue, TResult> onSome,
        Func<Error, TResult> onNone)
    {
        Maybe<TValue, Error> fullMaybe = maybe;
        return fullMaybe.Match(onSome, onNone);
    }

    public static async Task<TResult> Match<TValue, TResult>(
        this Task<Maybe<TValue>>maybetask,
        Func<TValue, TResult> onSome,
        Func<Error, TResult> onNone)
    {
        Maybe<TValue, Error> fullMaybe = await maybetask;
        return fullMaybe.Match(onSome, onNone);
    }

    /// <summary>
    /// Asynchronously transforms the Maybe by applying one of two functions, depending on the outcome's state.
    /// </summary>
    public static async Task<TResult> MatchAsync<TValue, TError, TResult>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<TResult>> onSome,
        Func<TError, Task<TResult>> onNone)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.Match(onSome, onNone).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously transforms the Maybe by applying one of two functions, depending on the outcome's state.
    /// </summary>
    public static async Task<TResult> MatchAsync<TValue, TResult>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, Task<TResult>> onSome,
        Func<Error, Task<TResult>> onNone)
    {
        Maybe<TValue, Error> fullMaybe = await maybeTask.ConfigureAwait(false);
        return await fullMaybe.Match(onSome, onNone).ConfigureAwait(false);
    }
}

