namespace Maybe;

public static partial class MaybeExtensions
{
    /// <summary>
    /// Returns the success value if the outcome is a success, otherwise returns the provided fallback value.
    /// This is a terminal operation that exits the Maybe context.
    /// </summary>
    /// <param name="maybe">The Maybe instance.</param>
    /// <param name="fallbackValue">The value to return if the outcome is an error.</param>
    /// <returns>The success value or the fallback value.</returns>
    public static TValue Else<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        TValue fallbackValue)
        where TError : IError
    {
        return maybe.IsSuccess
            ? maybe.ValueOrThrow()
            : fallbackValue;
    }

    public static TValue Else<TValue>(
        this in Maybe<TValue> maybe,
        TValue fallbackValue)
    {
        return maybe.IsSuccess
            ? maybe.ValueOrThrow()
            : fallbackValue;
    }

    /// <summary>
    /// Returns the success value if the outcome is a success, otherwise invokes the fallback function and returns its result.
    /// This is a terminal operation that exits the Maybe context.
    /// </summary>
    /// <param name="maybe">The Maybe instance.</param>
    /// <param name="fallbackFunc">The function to invoke to generate a fallback value if the outcome is an error.</param>
    /// <returns>The success value or the result of the fallback function.</returns>
    public static TValue Else<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Func<TError, TValue> fallbackFunc)
        where TError : IError
    {
        return maybe.IsSuccess
            ? maybe.ValueOrThrow()
            : fallbackFunc(maybe.ErrorOrThrow());
    }

    public static TValue Else<TValue>(
        this in Maybe<TValue> maybe,
        Func<Error, TValue> fallbackFunc)
    {
        return maybe.IsSuccess
            ? maybe.ValueOrThrow()
            : fallbackFunc((Error)maybe.ErrorOrThrow());
    }

    /// <summary>
    /// Asynchronously returns the success value if the outcome is a success, otherwise returns the provided fallback value.
    /// This is a terminal operation that exits the Maybe context.
    /// </summary>
    /// <param name="maybeTask">The task representing the Maybe instance.</param>
    /// <param name="fallbackValue">The value to return if the outcome is an error.</param>
    /// <returns>A task representing the success value or the fallback value.</returns>
    public static async Task<TValue> Else<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        TValue fallbackValue)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Else(fallbackValue);
    }

    public static async Task<TValue> Else<TValue>(
        this Task<Maybe<TValue>> maybeTask,
        TValue fallbackValue)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Else(fallbackValue);
    }

    /// <summary>
    /// Asynchronously returns the success value if the outcome is a success, otherwise invokes the fallback function and returns its result.
    /// This is a terminal operation that exits the Maybe context.
    /// </summary>
    /// <param name="maybeTask">The task representing the Maybe instance.</param>
    /// <param name="fallbackFunc">The function to invoke to generate a fallback value if the outcome is an error.</param>
    /// <returns>A task representing the success value or the result of the fallback function.</returns>
    public static async Task<TValue> Else<TValue, TError>(
        this Task<Maybe<TValue, TError>> maybeTask,
        Func<TError, TValue> fallbackFunc)
        where TError : IError
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Else(fallbackFunc);
    }

    public static async Task<TValue> Else<TValue>(
        this Task<Maybe<TValue>> maybeTask,
        Func<Error, TValue> fallbackFunc)
    {
        var maybe = await maybeTask.ConfigureAwait(false);
        return maybe.Else(fallbackFunc);
    }
}
