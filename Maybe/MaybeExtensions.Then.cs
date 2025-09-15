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
    /// Sync -> Sync | Specific BaseError -> Specific BaseError
    /// </summary>
    public static Maybe<TNewValue, TNewError> Then<TValue, TError, TNewValue, TNewError>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, Maybe<TNewValue, TNewError>> func)
        where TError : BaseError, new() where TNewError : BaseError, new()
    {
        if (maybe.IsSuccess)
        {
            return func(maybe.ValueOrThrow());
        }
        else
        {
            if (maybe.ErrorOrThrow() is TNewError sameTypeError)
            {
                return Maybe<TNewValue, TNewError>.None(sameTypeError);
            }
            else
            {
                return Maybe<TNewValue, TNewError>.None(CreateErrorFrom<TNewError>(maybe.ErrorOrThrow()));
            }
        }
    }

    /// <summary>
    /// Sync -> Sync | Specific BaseError -> Same BaseError
    /// </summary>
    public static Maybe<TValue, TError> Then<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, Maybe<TValue, TError>> func)
        where TError : BaseError, new() 
    {
        return maybe.Then<TValue, TError, TValue, TError>(func);
    }

    /// <summary>
    /// Sync -> Async | Specific BaseError -> Specific BaseError
    /// </summary>
    public static async Task<Maybe<TNewValue, TNewError>> ThenAsync<TValue, TError, TNewValue, TNewError>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, Task<Maybe<TNewValue, TNewError>>> funcAsync)
        where TError : BaseError, new() where TNewError : BaseError, new()
    {
        if (maybe.IsSuccess) {
            return await funcAsync(maybe.ValueOrThrow()).ConfigureAwait(false);
        }
        else if (maybe.ErrorOrThrow() is TNewError sameTypeError) {
            return Maybe<TNewValue, TNewError>.None(sameTypeError);
        }
        else {
            return Maybe<TNewValue, TNewError>.None(CreateErrorFrom<TNewError>(maybe.ErrorOrThrow()));
        }
    }

    /// <summary>
    /// Sync -> Async | Specific BaseError -> Same BaseError
    /// </summary>
    public static Task<Maybe<TValue, TError>> ThenAsync<TValue, TError>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, Task<Maybe<TValue, TError>>> funcAsync)
        where TError : BaseError, new()
    {
        return maybe.ThenAsync<TValue, TError, TValue, TError>(funcAsync);
    }

    #endregion

    #region Source: Task<Maybe<T, E1>> (Async)

    /// <summary>
    /// Async -> Sync | Specific BaseError -> Specific BaseError
    /// </summary>
    public static async Task<Maybe<TNewValue, TNewError>> Then<TValue, TError, TNewValue, TNewError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Maybe<TNewValue, TNewError>> func)
        where TError : BaseError, new() where TNewError : BaseError, new()
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Then(func);
    }

    /// <summary>
    /// Async -> Sync | Specific BaseError -> Same BaseError
    /// </summary>
    public static Task<Maybe<TValue, TError>> Then<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Maybe<TValue, TError>> func)
        where TError : BaseError, new()
    {
        return maybeTask.Then<TValue, TError, TValue, TError>(func);
    }

    /// <summary>
    /// Async -> Async | Specific BaseError -> Specific BaseError
    /// </summary>
    public static async Task<Maybe<TNewValue, TNewError>> ThenAsync<TValue, TError, TNewValue, TNewError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<Maybe<TNewValue, TNewError>>> funcAsync)
        where TError : BaseError, new() where TNewError : BaseError, new()
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.ThenAsync(funcAsync).ConfigureAwait(false);
    }

    /// <summary>
    /// Async -> Async | Specific BaseError -> Same BaseError
    /// </summary>
    public static Task<Maybe<TValue, TError>> ThenAsync<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<Maybe<TValue, TError>>> funcAsync)
        where TError : BaseError, new()
    {
        return maybeTask.ThenAsync<TValue, TError, TValue, TError>(funcAsync);
    }

    #endregion

    /// <summary>
    /// Safely creates a new error instance from a source error, using the BaseError(BaseError other) constructor.
    /// </summary>
    private static TNewError CreateErrorFrom<TNewError>(BaseError sourceError) where TNewError : BaseError, new()
    {
        var newError = new TNewError();

        newError.SetInnerError(sourceError);

        return newError;
    }
}

