namespace Maybe;

public static partial class MaybeExtensions
{
    /// <summary>
    /// Ensures that the success value satisfies a given condition. If the condition is not met, returns the specified error.
    /// </summary>
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

        return predicate(maybe.ValueOrThrow()) ? maybe : error;
    }

    /// <summary>
    /// Ensures that the success value satisfies a given condition. If the condition is not met, returns the specified error.
    /// </summary>
    public static Maybe<TValue, TError> Ensure<TValue, TError>(
        this in Maybe<TValue> maybe,
        Func<TValue, bool> predicate,
        TError error)
        where TError : IError
    {
        Maybe<TValue, TError> fullMaybe = maybe.IsSuccess
            ? Maybe<TValue, TError>.Some(maybe.ValueOrThrow())
            : Maybe<TValue, TError>.None((TError)maybe.ErrorOrThrow());
        return fullMaybe.Ensure(predicate, error);
    }

    public static Maybe<TValue, Error> Ensure<TValue>(
        this in Maybe<TValue> maybe,
        Func<TValue, bool> predicate,
        Error error)
    {
        Maybe<TValue, Error> fullMaybe = maybe;
        return fullMaybe.Ensure(predicate, error);
    }

    /// <summary>
    /// Asynchronously ensures that the success value satisfies a given condition.
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
    /// Asynchronously ensures that the success value satisfies a given condition.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> Ensure<TValue, TError>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, bool> predicate,
        TError error)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);  
        Maybe<TValue, TError> fullMaybe = maybe.IsSuccess
            ? Maybe<TValue, TError>.Some(maybe.ValueOrThrow())
            : Maybe<TValue, TError>.None((TError)maybe.ErrorOrThrow());
        return fullMaybe.Ensure(predicate, error);
    }

    /// <summary>
    /// Asynchronously ensures that the success value satisfies a given condition.
    /// </summary>
    public static async Task<Maybe<TValue, Error>> Ensure<TValue>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, bool> predicate,
        Error error)
    {
        Maybe<TValue, Error> fullMaybe = await maybeTask.ConfigureAwait(false);
        return fullMaybe.Ensure(predicate, error);
    }

    /// <summary>
    /// Asynchronously ensures that the success value satisfies a given asynchronous condition.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> EnsureAsync<TValue, TError>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, Task<bool>> predicate,
        TError error)
        where TError : IError
    {
        if (maybe.IsError)
        {
            return maybe;
        }

        return await predicate(maybe.ValueOrThrow()).ConfigureAwait(false) ? maybe : error;
    }

    /// <summary>
    /// Asynchronously ensures that the success value satisfies a given asynchronous condition.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> EnsureAsync<TValue, TError>(
        this Maybe<TValue> maybe,
        Func<TValue, Task<bool>> predicate,
        TError error)
        where TError : IError
    {
        Maybe<TValue, TError> fullMaybe = maybe.IsSuccess
            ? Maybe<TValue, TError>.Some(maybe.ValueOrThrow())
            : Maybe<TValue, TError>.None((TError)maybe.ErrorOrThrow());
        return await fullMaybe.EnsureAsync(predicate, error).ConfigureAwait(false);
    }

    public static async Task<Maybe<TValue, Error>> EnsureAsync<TValue>(
        this Maybe<TValue> maybe,
        Func<TValue, Task<bool>> predicate,
        Error error)
    {
        Maybe<TValue, Error> fullMaybe = maybe;
        return await fullMaybe.EnsureAsync(predicate, error).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously ensures that the success value satisfies a given asynchronous condition.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> EnsureAsync<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<bool>> predicate,
        TError error)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.EnsureAsync(predicate, error).ConfigureAwait(false);
    }

    /// <summary>
    /// Asynchronously ensures that the success value satisfies a given asynchronous condition.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> EnsureAsync<TValue, TError>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, Task<bool>> predicate,
        TError error)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        Maybe<TValue, TError> fullMaybe = maybe.IsSuccess
            ? Maybe<TValue, TError>.Some(maybe.ValueOrThrow())
            : Maybe<TValue, TError>.None((TError)maybe.ErrorOrThrow());
        return await fullMaybe.EnsureAsync(predicate, error).ConfigureAwait(false);
    }
}

