namespace Maybe;

/// <summary>
/// Provides ergonomic extension methods for validating the success value of a Maybe,
/// preserving the specific error type channel.
/// </summary>
public static partial class MaybeExtensions
{
    /// <summary>
    /// If the outcome is a success, checks if the value satisfies a predicate. If not, it returns the provided error of the same type.
    /// This preserves the specific error channel.
    /// </summary>
    public static Maybe<TValue, TError> Ensure<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Func<TValue, bool> predicate,
        TError error)
        where TError : Error
    {
        if (maybe.IsError) return maybe;
        return predicate(maybe.ValueOrThrow()) ? maybe : error;
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then applies the Ensure validation, preserving the specific error type.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> Ensure<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, bool> predicate,
        TError error)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Ensure(predicate, error);
    }

    /// <summary>
    /// If the outcome is a success, asynchronously checks if the value satisfies a predicate, preserving the specific error type.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> EnsureAsync<TValue, TError>(
        this Maybe<TValue, TError> maybe,
        Func<TValue, Task<bool>> predicateAsync,
        TError error)
        where TError : Error
    {
        if (maybe.IsError) return maybe;
        return await predicateAsync(maybe.ValueOrThrow()).ConfigureAwait(false) ? maybe : error;
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then applies the asynchronous Ensure validation, preserving the specific error type.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> EnsureAsync<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<bool>> predicateAsync,
        TError error)
        where TError : Error
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.EnsureAsync(predicateAsync, error).ConfigureAwait(false);
    }
}
