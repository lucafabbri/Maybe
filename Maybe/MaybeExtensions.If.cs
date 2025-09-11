namespace Maybe;

public static partial class MaybeExtensions
{
    /// <summary>
    /// Executes an action on the success value if the outcome is a success. Returns the original Maybe instance.
    /// </summary>
    public static Maybe<TValue, TError> IfSome<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Action<TValue> action)
        where TError : IError
    {
        if (maybe.IsSuccess)
        {
            action(maybe.ValueOrThrow());
        }
        return maybe;
    }

    /// <summary>
    /// Executes an action on the success value if the outcome is a success. Returns the original Maybe instance.
    /// </summary>
    public static Maybe<TValue> IfSome<TValue>(
        this in Maybe<TValue> maybe,
        Action<TValue> action)
    {
        if (maybe.IsSuccess)
        {
            action(maybe.ValueOrThrow());
        }
        return maybe;
    }

    /// <summary>
    /// Executes an action on the error if the outcome is an error. Returns the original Maybe instance.
    /// </summary>
    public static Maybe<TValue, TError> IfNone<TValue, TError>(
        this in Maybe<TValue, TError> maybe,
        Action<TError> action)
        where TError : IError
    {
        if (maybe.IsError)
        {
            action(maybe.ErrorOrThrow());
        }
        return maybe;
    }

    /// <summary>
    /// Executes an action on the error if the outcome is an error. Returns the original Maybe instance.
    /// </summary>
    public static Maybe<TValue> IfNone<TValue>(
        this in Maybe<TValue> maybe,
        Action<Error> action)
    {
        if (maybe.IsError)
        {
            action((Error)maybe.ErrorOrThrow());
        }
        return maybe;
    }
}

