namespace Maybe
{
    public interface IError : IEquatable<IError>
    {
        string Code { get; }
        string Message { get; }
        int TimeStamp { get; }
        OutcomeType Type { get; }
    }
}