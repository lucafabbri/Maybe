using System.Text.Json;
using Maybe;
using Maybe.Toolkit;

namespace Maybe.Toolkit.Demo;

public static class Program
{
    public static async Task Main()
    {
        Console.WriteLine("=== Maybe Toolkit Demo ===\n");

        await DemoJsonToolkit();
        Console.WriteLine();

        await DemoFileToolkit();
        Console.WriteLine();

        DemoParseToolkit();
        Console.WriteLine();

        await DemoHttpToolkit();
        Console.WriteLine();

        DemoCollectionToolkit();
        Console.WriteLine();

        Console.WriteLine("=== Demo Complete ===");
    }

    private static async Task DemoJsonToolkit()
    {
        Console.WriteLine("--- JSON Toolkit Demo ---");

        // Success case
        var person = new { Name = "John Doe", Age = 30, City = "New York" };
        var serializeResult = JsonToolkit.TrySerialize(person);
        
        serializeResult
            .IfSome(json => Console.WriteLine($"✓ Serialized: {json}"))
            .ElseDo(error => Console.WriteLine($"✗ Serialization failed: {error.Message}"));

        if (serializeResult.IsSuccess)
        {
            var json = serializeResult.ValueOrThrow();
            var deserializeResult = JsonToolkit.TryDeserialize<dynamic>(json);
            
            deserializeResult
                .IfSome(obj => Console.WriteLine($"✓ Deserialized successfully"))
                .ElseDo(error => Console.WriteLine($"✗ Deserialization failed: {error.Message}"));
        }

        // Error case
        var invalidJson = "{\"Name\":\"John\",\"Age\":}";
        JsonToolkit.TryDeserialize<object>(invalidJson)
            .IfSome(_ => Console.WriteLine("✓ This shouldn't happen"))
            .ElseDo(error => Console.WriteLine($"✓ Expected error: {error.Message}"));
    }

    private static async Task DemoFileToolkit()
    {
        Console.WriteLine("--- File Toolkit Demo ---");

        var tempFile = Path.GetTempFileName();
        var content = "Hello, Maybe Toolkit!";

        try
        {
            // Write file
            var writeResult = FileToolkit.TryWriteAllText(tempFile, content);
            writeResult
                .IfSome(_ => Console.WriteLine($"✓ File written to: {tempFile}"))
                .ElseDo(error => Console.WriteLine($"✗ Write failed: {error.Message}"));

            // Read file
            var readResult = FileToolkit.TryReadAllText(tempFile);
            readResult
                .IfSome(text => Console.WriteLine($"✓ File content: {text}"))
                .ElseDo(error => Console.WriteLine($"✗ Read failed: {error.Message}"));

            // Read non-existent file
            FileToolkit.TryReadAllText("/path/to/nonexistent/file.txt")
                .IfSome(_ => Console.WriteLine("✗ This shouldn't happen"))
                .ElseDo(error => Console.WriteLine($"✓ Expected error: File not found"));
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    private static void DemoParseToolkit()
    {
        Console.WriteLine("--- Parse Toolkit Demo ---");

        // Success cases
        ParseToolkit.TryParseInt("42")
            .IfSome(value => Console.WriteLine($"✓ Parsed int: {value}"))
            .ElseDo(error => Console.WriteLine($"✗ Parse failed: {error.Message}"));

        ParseToolkit.TryParseDouble("3.14159")
            .IfSome(value => Console.WriteLine($"✓ Parsed double: {value}"))
            .ElseDo(error => Console.WriteLine($"✗ Parse failed: {error.Message}"));

        ParseToolkit.TryParseGuid(Guid.NewGuid().ToString())
            .IfSome(value => Console.WriteLine($"✓ Parsed GUID: {value}"))
            .ElseDo(error => Console.WriteLine($"✗ Parse failed: {error.Message}"));

        // Error cases
        ParseToolkit.TryParseInt("not-a-number")
            .IfSome(_ => Console.WriteLine("✗ This shouldn't happen"))
            .ElseDo(error => Console.WriteLine($"✓ Expected error: {error.Message}"));

        ParseToolkit.TryParseDouble("invalid-double")
            .IfSome(_ => Console.WriteLine("✗ This shouldn't happen"))
            .ElseDo(error => Console.WriteLine($"✓ Expected error: {error.Message}"));
    }

    private static async Task DemoHttpToolkit()
    {
        Console.WriteLine("--- HTTP Toolkit Demo ---");

        using var client = new HttpClient();

        // Demo with invalid URL to show error handling
        var result = await client.TryGetAsync("invalid-url");
        result
            .IfSome(response => Console.WriteLine($"✓ Response received: {response.StatusCode}"))
            .ElseDo(error => Console.WriteLine($"✓ Expected error: HTTP request failed"));

        // Demo with null client
        HttpClient? nullClient = null;
        var nullResult = await nullClient!.TryGetAsync("http://example.com");
        nullResult
            .IfSome(_ => Console.WriteLine("✗ This shouldn't happen"))
            .ElseDo(error => Console.WriteLine($"✓ Expected error: HttpClient cannot be null"));
    }

    private static void DemoCollectionToolkit()
    {
        Console.WriteLine("--- Collection Toolkit Demo ---");

        // Dictionary demo
        IDictionary<string, int> dictionary = new Dictionary<string, int>
        {
            { "apple", 5 },
            { "banana", 3 },
            { "orange", 8 }
        };

        dictionary.TryGetValue("apple")
            .IfSome(value => Console.WriteLine($"✓ Found apple: {value}"))
            .ElseDo(error => Console.WriteLine($"✗ Failed: {error.Message}"));

        dictionary.TryGetValue("grape")
            .IfSome(_ => Console.WriteLine("✗ This shouldn't happen"))
            .ElseDo(error => Console.WriteLine($"✓ Expected error: Key 'grape' was not found"));

        // List demo
        IList<string> fruits = new List<string> { "apple", "banana", "orange" };

        fruits.TryGetAt(1)
            .IfSome(fruit => Console.WriteLine($"✓ Fruit at index 1: {fruit}"))
            .ElseDo(error => Console.WriteLine($"✗ Failed: {error.Message}"));

        fruits.TryGetAt(10)
            .IfSome(_ => Console.WriteLine("✗ This shouldn't happen"))
            .ElseDo(error => Console.WriteLine($"✓ Expected error: Index out of range"));

        // First/Last demo
        fruits.TryFirst()
            .IfSome(first => Console.WriteLine($"✓ First fruit: {first}"))
            .ElseDo(error => Console.WriteLine($"✗ Failed: {error.Message}"));

        fruits.TryLast()
            .IfSome(last => Console.WriteLine($"✓ Last fruit: {last}"))
            .ElseDo(error => Console.WriteLine($"✗ Failed: {error.Message}"));

        // Empty collection demo
        var emptyList = new List<string>();
        emptyList.TryFirst()
            .IfSome(_ => Console.WriteLine("✗ This shouldn't happen"))
            .ElseDo(error => Console.WriteLine($"✓ Expected error: Sequence contains no elements"));
    }
}