namespace Maybe;

public static partial class MaybeExtensions
{
    public static Maybe<TValue> MightBe<TValue>(this TValue value) => Maybe<TValue>.Some(value);
    public static Maybe<TValue> MightBe<TValue>(this Error error) => Maybe<TValue>.None(error);
    public static Maybe<TValue, TError> MightBe<TValue, TError>(this TValue value) where TError : IError => Maybe<TValue, TError>.Some(value);
    public static Maybe<TValue, TError> MightBe<TValue, TError>(this TError error) where TError : IError => Maybe<TValue, TError>.None(error);
}

