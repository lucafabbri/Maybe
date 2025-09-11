namespace Maybe;

public static partial class MaybeExtensions
{
    /// <summary>
    /// If the outcome is a success, applies a function that returns a Maybe to the value.
    /// This is used to chain operations that can fail.
    /// </summary>
    public static Maybe<TNewValue, TError> Then<TValue, TError, TNewValue>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, Maybe<TNewValue, TError>> func)
        where TError : IError
    {
        return maybe.IsSuccess
            ? func(maybe.ValueOrThrow())
            : Maybe<TNewValue, TError>.None(maybe.ErrorOrThrow());
    }

    public static Maybe<TNewValue> Then<TValue, TError, TNewValue>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, Maybe<TNewValue>> func)
        where TError : IError
    {
        if(maybe.IsSuccess)
        {
            var result = func(maybe.ValueOrThrow());
            return result;
        }
        else
        {
            var error = maybe.ErrorOrThrow();

            return Maybe<TNewValue>.None(Error.Custom(error.Type, error.Code, error.Message));
        }
    }

    /// <summary>
    /// If the outcome is a success, applies a function that returns a Maybe to the value.
    /// </summary>
    public static Maybe<TNewValue, Error> Then<TValue, TNewValue>(
        this in Maybe<TValue> maybe,
        Func<TValue, Maybe<TNewValue>> func)
    {
        Maybe<TValue, Error> fullMaybe = maybe;
        var result = fullMaybe.Then(value => (Maybe<TNewValue, Error>)func(value));
        return result.IsSuccess
            ? Maybe<TNewValue>.Some(result.ValueOrThrow())
            : Maybe<TNewValue>.None(result.ErrorOrThrow());
    }

    /// <summary>
    /// Asynchronously applies a function that returns a Maybe to the value of a Maybe.
    /// </summary>
    public static async Task<Maybe<TNewValue, TError>> Then<TValue, TError, TNewValue>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Maybe<TNewValue, TError>> func)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Then(func);
    }

    public static async Task<Maybe<TNewValue>> Then<TValue, TError, TNewValue>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Maybe<TNewValue>> func)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Then(func);
    }

    /// <summary>
    /// Asynchronously applies a function that returns a Maybe to the value of a Maybe.
    /// </summary>
    public static async Task<Maybe<TNewValue, Error>> Then<TValue, TNewValue>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, Maybe<TNewValue>> func)
    {
        Maybe<TValue, Error> fullMaybe = await maybeTask.ConfigureAwait(false);
        return fullMaybe.Then(value => (Maybe<TNewValue, Error>)func(value));
    }

    /// <summary>
    /// Asynchronously applies an asynchronous function that returns a Maybe to the value of a Maybe.
    /// </summary>
    public static async Task<Maybe<TNewValue, TError>> ThenAsync<TValue, TError, TNewValue>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, Task<Maybe<TNewValue, TError>>> func)
        where TError : IError
    {
        return maybe.IsSuccess
            ? await func(maybe.ValueOrThrow()).ConfigureAwait(false)
            : Maybe<TNewValue, TError>.None(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Asynchronously applies an asynchronous function that returns a Maybe to the value of a Maybe.
    /// </summary>
    public static async Task<Maybe<TNewValue, Error>> ThenAsync<TValue, TNewValue>(
        this Maybe<TValue> maybe,
        Func<TValue, Task<Maybe<TNewValue>>> func)
    {
        return maybe.IsSuccess
            ? await func(maybe.ValueOrThrow()).ConfigureAwait(false)
            : Maybe<TNewValue, Error>.None((Error)maybe.ErrorOrThrow());
    }

    public static async Task<Maybe<TNewValue, Error>> ThenAsync<TValue, TNewValue>(
        this Maybe<TValue> maybe,
        Func<TValue, Task<Maybe<TNewValue, Error>>> func)
    {
        return maybe.IsSuccess
            ? await func(maybe.ValueOrThrow()).ConfigureAwait(false)
            : Maybe<TNewValue, Error>.None((Error)maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Asynchronously applies an asynchronous function that returns a Maybe to the value of a Maybe.
    /// </summary>
    public static async Task<Maybe<TNewValue, TError>> ThenAsync<TValue, TError, TNewValue>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<Maybe<TNewValue, TError>>> func)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.ThenAsync(func).ConfigureAwait(false);
    }

    public static async Task<Maybe<TNewValue, Error>> ThenAsync<TValue, TNewValue>(
        this Task<Maybe<TValue, Error>> maybeTask,
        Func<TValue, Task<Maybe<TNewValue, Error>>> func)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.ThenAsync(func).ConfigureAwait(false);
    }

    public static async Task<Maybe<TNewValue>> ThenAsync<TValue, TNewValue>(
        this Task<Maybe<TValue, Error>> maybeTask,
        Func<TValue, Task<Maybe<TNewValue>>> func)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.IsSuccess
            ? await func(maybe.ValueOrThrow()).ConfigureAwait(false)
            : Maybe<TNewValue>.None(maybe.ErrorOrThrow());
    }

    public static async Task<Maybe<TNewValue>> ThenAsync<TValue, TNewValue>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, Task<Maybe<TNewValue>>> func)
    {
        var maybe = (Maybe<TValue, Error>)await maybeTask.ConfigureAwait(false);
        return maybe.IsSuccess
            ? await func(maybe.ValueOrThrow()).ConfigureAwait(false)
            : Maybe<TNewValue>.None(maybe.ErrorOrThrow());
    }
}

