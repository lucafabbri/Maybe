namespace Maybe.Tests;

/// <summary>
/// Provides shared data and types for unit tests to ensure consistency and reduce repetition.
/// </summary>
public static class TestData
{
    public record TestValue(int Id, string Name);

    public record TestCustomError(string Detail) : IError
    {
        public OutcomeType Type => OutcomeType.Failure;
        public string Code => "Test.Custom";
        public string Message => "A custom test error occurred.";
    }

    public static TestValue SuccessValue => new(1, "Success");
    public static Error TestError => Error.Failure("Test.Failure", "A test failure occurred.");
    public static TestCustomError CustomError => new("Custom detail");
}

public record User(int Id, string Name);