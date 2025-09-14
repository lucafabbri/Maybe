namespace Maybe.Tests;

/// <summary>
/// Provides shared data and types for unit tests to ensure consistency and reduce repetition.
/// </summary>
public static class TestData
{
    public static TestValue SuccessValue => new(1, "Success");
    public static FailureError TestError => new("Test.Failure", "A test failure occurred.");
    public static TestCustomError CustomError => new("Custom detail");
}
