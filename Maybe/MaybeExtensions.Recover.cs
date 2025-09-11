namespace Maybe;

public static partial class MaybeExtensions
{
    /// <summary>
    /// If the outcome is an error, applies a recovery function that returns a new Maybe.
    /// This allows for fallback operations.
    /// </summary>
    public static Maybe<TValue, TError> Recover<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Func<TError, Maybe<TValue, TError>> recoveryFunc)
        where TError : Error
    {
        return maybe.IsError ? recoveryFunc(maybe.ErrorOrThrow()) : maybe;
    }

    /// <summary>
    /// If the outcome is an error, applies a recovery function that returns a new Maybe.
    /// </summary>
    public static Maybe<TValue, TError> Recover<TValue, TError>(
        this in Maybe<TValue> maybe,
        Func<TError, Maybe<TValue, TError>> recoveryFunc)
        where TError : Error
    {
        Maybe<TValue, TError> fullMaybe = maybe.IsSuccess
            ? Maybe<TValue, TError>.Some(maybe.ValueOrThrow())
            : Maybe<TValue, TError>.None((TError)maybe.ErrorOrThrow());
        return fullMaybe.Recover(recoveryFunc);
    }

    public static Maybe<TValue, Error> Recover<TValue>(
        this in Maybe<TValue> maybe,
        Func<Error, Maybe<TValue, Error>> recoveryFunc)
    {
        Maybe<TValue, Error> fullMaybe = maybe;
        return fullMaybe.Recover(recoveryFunc);
    }

    /// <summary>
    /// Asynchronously applies a recovery function if the outcome is an error.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> Recover<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, Maybe<TValue, TError>> recoveryFunc)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Recover(recoveryFunc);
    }

    /// <summary>
    /// Asynchronously applies a recovery function if the outcome is an error.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> Recover<TValue, TError>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TError, Maybe<TValue, TError>> recoveryFunc)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);  
        Maybe<TValue, TError> fullMaybe = maybe.IsSuccess
            ? Maybe<TValue, TError>.Some(maybe.ValueOrThrow())
            : Maybe<TValue, TError>.None((TError)maybe.ErrorOrThrow());
        return fullMaybe.Recover(recoveryFunc);
    }

    public static async Task<Maybe<TValue, Error>> Recover<TValue>(
        this Task<Maybe<TValue>> maybeTask,
        Func<Error, Maybe<TValue, Error>> recoveryFunc)
    {
        Maybe<TValue, Error> fullMaybe = await maybeTask.ConfigureAwait(false);
        return fullMaybe.Recover(recoveryFunc);
    }

    /// <summary>
    /// Asynchronously applies an asynchronous recovery function if the outcome is an error.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> RecoverAsync<TValue, TError>(
        this Maybe<TValue, TError> maybe,
        Func<TError, Task<Maybe<TValue, TError>>> recoveryFunc)
        where TError : Error
    {
        return maybe.IsError ? await recoveryFunc(maybe.ErrorOrThrow()).ConfigureAwait(false) : maybe;
    }

    /// <summary>
    /// Asynchronously applies an asynchronous recovery function if the outcome is an error.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> RecoverAsync<TValue, TError>(
        this Maybe<TValue> maybe,
        Func<TError, Task<Maybe<TValue, TError>>> recoveryFunc)
        where TError : Error
    {
        Maybe<TValue, TError> fullMaybe = maybe.IsSuccess
            ? Maybe<TValue, TError>.Some(maybe.ValueOrThrow())
            : Maybe<TValue, TError>.None((TError)maybe.ErrorOrThrow());
        return await fullMaybe.RecoverAsync(recoveryFunc).ConfigureAwait(false);
    }

    public static async Task<Maybe<TValue, Error>> RecoverAsync<TValue>(
        this Maybe<TValue> maybe,
        Func<Error, Task<Maybe<TValue, Error>>> recoveryFunc)
    {
        Maybe<TValue, Error> fullMaybe = maybe;
        return await fullMaybe.RecoverAsync(recoveryFunc).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously applies an asynchronous recovery function if the outcome is an error.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> RecoverAsync<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, Task<Maybe<TValue, TError>>> recoveryFunc)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.RecoverAsync(recoveryFunc).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously applies an asynchronous recovery function if the outcome is an error.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> RecoverAsync<TValue, TError>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TError, Task<Maybe<TValue, TError>>> recoveryFunc)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        Maybe<TValue, TError> fullMaybe = maybe.IsSuccess
            ? Maybe<TValue, TError>.Some(maybe.ValueOrThrow())
            : Maybe<TValue, TError>.None((TError)maybe.ErrorOrThrow());
        return await fullMaybe.RecoverAsync(recoveryFunc).ConfigureAwait(false);
    }

    public static async Task<Maybe<TValue, Error>> RecoverAsync<TValue>(
        this Task<Maybe<TValue>> maybeTask,
        Func<Error, Task<Maybe<TValue, Error>>> recoveryFunc)
    {
        Maybe<TValue, Error> fullMaybe = await maybeTask.ConfigureAwait(false);
        return await fullMaybe.RecoverAsync(recoveryFunc).ConfigureAwait(false);
    }
}

