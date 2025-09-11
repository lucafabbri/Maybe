namespace Maybe;

/// <summary>
/// A compatibility type that mirrors the public API of ErrorOr.Result.
/// </summary>
public static class Result
{
    public static ErrorOr<Success> Success => Maybe.Outcomes.Success;
    public static ErrorOr<Created> Created => Maybe.Outcomes.Created;
    public static ErrorOr<Deleted> Deleted => Maybe.Outcomes.Deleted;
    public static ErrorOr<Updated> Updated => Maybe.Outcomes.Updated;
}
