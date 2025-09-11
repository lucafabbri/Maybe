namespace Maybe.Tests;

/// <summary>
/// Provides shared data and types for unit tests to ensure consistency and reduce repetition.
/// </summary>
public static class TestData
{
    public record TestValue(int Id, string Name);

    public class TestCustomError : Error    
    {
        public string Detail { get; }
        public TestCustomError(string detail) : base(OutcomeType.Failure, "Test.Custom", "A custom test error.") 
        { 
            Detail = detail;
        }
    }

    public static TestValue SuccessValue => new(1, "Success");
    public static Error TestError => Error.Failure("Test.Failure", "A test failure occurred.");
    public static TestCustomError CustomError => new("Custom detail");
}

public record User(int Id, string Name);