namespace Maybe;

public static partial class MaybeExtensions
{
    public static Maybe<TValue, TError> MightBe<TValue, TError>(this TValue value) where TError : BaseError, new() => Maybe<TValue, TError>.Some(value);
    public static Maybe<TValue, TError> MightBe<TValue, TError>(this TError error) where TError : BaseError, new() => Maybe<TValue, TError>.None(error);
}

