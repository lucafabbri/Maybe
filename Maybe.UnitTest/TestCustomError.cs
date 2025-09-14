namespace Maybe.Tests;

public class TestCustomError : FailureError
{
    public string Detail { get; set; }

    public TestCustomError() : base("Test.Custom", "A custom test error.") { }

    public TestCustomError(string detail) : base("Test.Custom", "A custom test error.")
    {
        Detail = detail;
    }
}
