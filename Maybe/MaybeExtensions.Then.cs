namespace Maybe;

/// <summary>
/// Provides 'Then' extension methods for the Maybe type to chain operations.
/// 'Then' (also known as Bind or FlatMap) is used to sequence operations that themselves return a Maybe.
/// This set of overloads is designed to maximize compiler type inference across synchronous and asynchronous contexts.
/// </summary>
public static partial class MaybeExtensions
{
    #region Source: Maybe<T, E1> (Sync)

    /// <summary>
    /// Sync -> Sync | Specific Error -> Specific Error
    /// </summary>
    public static Maybe<TNewValue, TNewError> Then<TValue, TError, TNewValue, TNewError>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, Maybe<TNewValue, TNewError>> func)
        where TError : Error where TNewError : Error
    {
        return maybe.IsSuccess
            ? func(maybe.ValueOrThrow())
            : Maybe<TNewValue, TNewError>.None(CreateErrorFrom<TNewError>(maybe.ErrorOrThrow()));
    }

    /// <summary>
    /// Sync -> Sync | Specific Error -> Default Error
    /// </summary>
    public static Maybe<TNewValue> Then<TValue, TError, TNewValue>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, Maybe<TNewValue>> func)
        where TError : Error
    {
        return maybe.IsSuccess
            ? func(maybe.ValueOrThrow())
            : Maybe<TNewValue>.None(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Sync -> Async | Specific Error -> Specific Error
    /// </summary>
    public static async Task<Maybe<TNewValue, TNewError>> ThenAsync<TValue, TError, TNewValue, TNewError>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, Task<Maybe<TNewValue, TNewError>>> funcAsync)
        where TError : Error where TNewError : Error
    {
        return maybe.IsSuccess
            ? await funcAsync(maybe.ValueOrThrow()).ConfigureAwait(false)
            : Maybe<TNewValue, TNewError>.None(CreateErrorFrom<TNewError>(maybe.ErrorOrThrow()));
    }

    /// <summary>
    /// Sync -> Async | Specific Error -> Default Error
    /// </summary>
    public static async Task<Maybe<TNewValue>> ThenAsync<TValue, TError, TNewValue>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, Task<Maybe<TNewValue>>> funcAsync)
        where TError : Error
    {
        return maybe.IsSuccess
            ? await funcAsync(maybe.ValueOrThrow()).ConfigureAwait(false)
            : Maybe<TNewValue>.None(maybe.ErrorOrThrow());
    }

    #endregion

    #region Source: Maybe<T> (Sync)

    /// <summary>
    /// Sync -> Sync | Default Error -> Specific Error
    /// </summary>
    public static Maybe<TNewValue, TNewError> Then<TValue, TNewValue, TNewError>(
        this in Maybe<TValue> maybe,
        Func<TValue, Maybe<TNewValue, TNewError>> func)
        where TNewError : Error
    {
        return maybe.IsSuccess
            ? func(maybe.ValueOrThrow())
            : Maybe<TNewValue, TNewError>.None(CreateErrorFrom<TNewError>(maybe.ErrorOrThrow()));
    }

    /// <summary>
    /// Sync -> Sync | Default Error -> Default Error
    /// </summary>
    public static Maybe<TNewValue> Then<TValue, TNewValue>(
        this in Maybe<TValue> maybe,
        Func<TValue, Maybe<TNewValue>> func)
    {
        return maybe.IsSuccess
            ? func(maybe.ValueOrThrow())
            : Maybe<TNewValue>.None(maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Sync -> Async | Default Error -> Specific Error
    /// </summary>
    public static async Task<Maybe<TNewValue, TNewError>> ThenAsync<TValue, TNewValue, TNewError>(
        this Maybe<TValue> maybe,
        Func<TValue, Task<Maybe<TNewValue, TNewError>>> funcAsync)
        where TNewError : Error
    {
        return maybe.IsSuccess
            ? await funcAsync(maybe.ValueOrThrow()).ConfigureAwait(false)
            : Maybe<TNewValue, TNewError>.None(CreateErrorFrom<TNewError>(maybe.ErrorOrThrow()));
    }

    /// <summary>
    /// Sync -> Async | Default Error -> Default Error
    /// </summary>
    public static async Task<Maybe<TNewValue>> ThenAsync<TValue, TNewValue>(
        this Maybe<TValue> maybe,
        Func<TValue, Task<Maybe<TNewValue>>> funcAsync)
    {
        return maybe.IsSuccess
            ? await funcAsync(maybe.ValueOrThrow()).ConfigureAwait(false)
            : Maybe<TNewValue>.None(maybe.ErrorOrThrow());
    }

    #endregion

    #region Source: Task<Maybe<T, E1>> (Async)

    /// <summary>
    /// Async -> Sync | Specific Error -> Specific Error
    /// </summary>
    public static async Task<Maybe<TNewValue, TNewError>> Then<TValue, TError, TNewValue, TNewError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Maybe<TNewValue, TNewError>> func)
        where TError : Error where TNewError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Then(func);
    }

    /// <summary>
    /// Async -> Sync | Specific Error -> Default Error
    /// </summary>
    public static async Task<Maybe<TNewValue>> Then<TValue, TError, TNewValue>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Maybe<TNewValue>> func)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Then(func);
    }

    /// <summary>
    /// Async -> Async | Specific Error -> Specific Error
    /// </summary>
    public static async Task<Maybe<TNewValue, TNewError>> ThenAsync<TValue, TError, TNewValue, TNewError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<Maybe<TNewValue, TNewError>>> funcAsync)
        where TError : Error where TNewError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.ThenAsync(funcAsync).ConfigureAwait(false);
    }

    /// <summary>
    /// Async -> Async | Specific Error -> Default Error
    /// </summary>
    public static async Task<Maybe<TNewValue>> ThenAsync<TValue, TError, TNewValue>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<Maybe<TNewValue>>> funcAsync)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.ThenAsync(funcAsync).ConfigureAwait(false);
    }

    #endregion

    #region Source: Task<Maybe<T>> (Async)

    /// <summary>
    /// Async -> Sync | Default Error -> Specific Error
    /// </summary>
    public static async Task<Maybe<TNewValue, TNewError>> Then<TValue, TNewValue, TNewError>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, Maybe<TNewValue, TNewError>> func)
        where TNewError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Then(func);
    }

    /// <summary>
    /// Async -> Sync | Default Error -> Default Error
    /// </summary>
    public static async Task<Maybe<TNewValue>> Then<TValue, TNewValue>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, Maybe<TNewValue>> func)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Then(func);
    }

    /// <summary>
    /// Async -> Async | Default Error -> Specific Error
    /// </summary>
    public static async Task<Maybe<TNewValue, TNewError>> ThenAsync<TValue, TNewValue, TNewError>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, Task<Maybe<TNewValue, TNewError>>> funcAsync)
        where TNewError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.ThenAsync(funcAsync).ConfigureAwait(false);
    }

    /// <summary>
    /// Async -> Async | Default Error -> Default Error
    /// </summary>
    public static async Task<Maybe<TNewValue>> ThenAsync<TValue, TNewValue>(
        this Task<Maybe<TValue>> maybeTask,
        Func<TValue, Task<Maybe<TNewValue>>> funcAsync)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.ThenAsync(funcAsync).ConfigureAwait(false);
    }

    #endregion

    /// <summary>
    /// Safely creates a new error instance from a source error, using the Error(Error other) constructor.
    /// </summary>
    private static TNewError CreateErrorFrom<TNewError>(Error sourceError) where TNewError : Error
    {
        if (typeof(TNewError) == sourceError.GetType() || typeof(TNewError) == typeof(Error))
        {
            return (TNewError)sourceError; // Safe direct cast
        }

        var newError = Activator.CreateInstance(typeof(TNewError), sourceError);
        if (newError is null)
        {
            // Fallback if Activator fails, to prevent null reference.
            return (TNewError)Error.Unexpected(
                "Activator.CreateInstance.Failed",
                $"Could not create an instance of error '{typeof(TNewError).Name}' from source '{sourceError.GetType().Name}'. Ensure it has a constructor that accepts a base Error type.");
        }
        return (TNewError)newError;
    }
}

