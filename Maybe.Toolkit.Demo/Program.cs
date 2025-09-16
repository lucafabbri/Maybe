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

        // Demo basic HTTP verbs with error handling
        Console.WriteLine("Basic HTTP Methods:");
        
        // GET Demo with invalid URL to show error handling
        var getResult = await client.TryGetAsync("invalid-url");
        getResult
            .IfSome(response => Console.WriteLine($"✓ GET Response received: {response.StatusCode}"))
            .ElseDo(error => Console.WriteLine($"✓ Expected GET error: HTTP request failed"));

        // POST Demo with null content
        var postResult = await client.TryPostAsync("invalid-url", null);
        postResult
            .IfSome(response => Console.WriteLine($"✓ POST Response received: {response.StatusCode}"))
            .ElseDo(error => Console.WriteLine($"✓ Expected POST error: HTTP request failed"));

        // PUT Demo
        var putResult = await client.TryPutAsync("invalid-url", null);
        putResult
            .IfSome(response => Console.WriteLine($"✓ PUT Response received: {response.StatusCode}"))
            .ElseDo(error => Console.WriteLine($"✓ Expected PUT error: HTTP request failed"));

        // PATCH Demo
        var patchResult = await client.TryPatchAsync("invalid-url", null);
        patchResult
            .IfSome(response => Console.WriteLine($"✓ PATCH Response received: {response.StatusCode}"))
            .ElseDo(error => Console.WriteLine($"✓ Expected PATCH error: HTTP request failed"));

        // DELETE Demo
        var deleteResult = await client.TryDeleteAsync("invalid-url");
        deleteResult
            .IfSome(response => Console.WriteLine($"✓ DELETE Response received: {response.StatusCode}"))
            .ElseDo(error => Console.WriteLine($"✓ Expected DELETE error: HTTP request failed"));

        Console.WriteLine("\nJSON Integration:");

        // JSON GET Demo
        var jsonGetResult = await client.TryGetJsonAsync<PersonDto>("invalid-url");
        jsonGetResult
            .IfSome(person => Console.WriteLine($"✓ Received person: {person.Name}"))
            .ElseDo(error => 
            {
                if (error.IsHttpError)
                    Console.WriteLine($"✓ Expected HTTP error in JSON GET: {error.HttpError?.Message}");
                else if (error.IsJsonError)
                    Console.WriteLine($"✓ Expected JSON error in JSON GET: {error.JsonError?.Message}");
            });

        // JSON POST Demo
        var samplePerson = new PersonDto { Name = "John Doe", Age = 30, Email = "john@example.com" };
        var jsonPostResult = await client.TryPostJsonAsync("invalid-url", samplePerson);
        jsonPostResult
            .IfSome(response => Console.WriteLine($"✓ JSON POST Response: {response.StatusCode}"))
            .ElseDo(error => 
            {
                if (error.IsHttpError)
                    Console.WriteLine($"✓ Expected HTTP error in JSON POST: Request failed");
                else if (error.IsJsonError)
                    Console.WriteLine($"✓ Expected JSON error in JSON POST: {error.JsonError?.Message}");
            });

        // JSON POST with response Demo
        var jsonPostWithResponseResult = await client.TryPostJsonAsync<PersonDto, PersonDto>("invalid-url", samplePerson);
        jsonPostWithResponseResult
            .IfSome(person => Console.WriteLine($"✓ Received response person: {person.Name}"))
            .ElseDo(error => 
            {
                if (error.IsHttpError)
                    Console.WriteLine($"✓ Expected HTTP error in JSON POST with response");
                else if (error.IsJsonError)
                    Console.WriteLine($"✓ Expected JSON error in JSON POST with response");
            });

        Console.WriteLine("\nNull Client Demo:");
        // Demo with null client
        HttpClient? nullClient = null;
        var nullResult = await nullClient!.TryGetJsonAsync<PersonDto>("http://example.com");
        nullResult
            .IfSome(_ => Console.WriteLine("✗ This shouldn't happen"))
            .ElseDo(error => Console.WriteLine($"✓ Expected error: HttpClient cannot be null"));
    }

    // Sample DTO for JSON demos
    public class PersonDto
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Email { get; set; } = string.Empty;
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