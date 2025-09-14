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
        Error error)
        where TError : Error, new()
    {
        if (maybe.IsError) return maybe;
        if (predicate(maybe.ValueOrThrow()))
        {
            return maybe;
        }
        else
        {
            var specificError = new TError();
            specificError.SetInnerError(error);
            return specificError;
        }
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then applies the Ensure validation, preserving the specific error type.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> Ensure<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, bool> predicate,
        Error error)
        where TError : Error, new()
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
        Error error)
        where TError : Error, new()
    {
        if (maybe.IsError) return maybe;
        if(await predicateAsync(maybe.ValueOrThrow()).ConfigureAwait(false))
        {             
            return maybe;
        }
        else
        {
            var specificError = new TError();
            specificError.SetInnerError(error);
            return specificError;
        }
    }

    /// <summary>
    /// Asynchronously awaits a Maybe and then applies the asynchronous Ensure validation, preserving the specific error type.
    /// </summary>
    public static async Task<Maybe<TValue, TError>> EnsureAsync<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TValue, Task<bool>> predicateAsync,
        Error error)
        where TError : Error, new()
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return await maybe.EnsureAsync(predicateAsync, error).ConfigureAwait(false);
    }
}
