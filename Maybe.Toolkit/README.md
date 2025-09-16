# Maybe.Toolkit

A comprehensive toolkit that extends the [Maybe](https://github.com/lucafabbri/Maybe) library with fluent, exception-free wrappers for common .NET operations.

## Overview

The Maybe.Toolkit provides safe, Maybe-based alternatives to traditional .NET operations that typically throw exceptions. Instead of using try-catch blocks, you can use fluent, functional programming patterns to handle potential failures gracefully.

## Features

### JsonToolkit
Safe JSON serialization and deserialization using `System.Text.Json`:
```csharp
// Traditional approach with try-catch
try 
{
    var person = JsonSerializer.Deserialize<Person>(json);
    // Process person...
}
catch (JsonException ex) 
{
    // Handle error...
}

// Maybe.Toolkit approach
JsonToolkit.TryDeserialize<Person>(json)
    .IfSome(person => Console.WriteLine($"Hello, {person.Name}!"))
    .ElseDo(error => Console.WriteLine($"Failed to parse JSON: {error.Message}"));
```

### FileToolkit
Safe file I/O operations:
```csharp
FileToolkit.TryReadAllText("config.json")
    .IfSome(content => ProcessConfig(content))
    .ElseDo(error => Console.WriteLine($"Failed to read config: {error.Message}"));
```

### ParseToolkit
Safe parsing operations for common types:
```csharp
ParseToolkit.TryParseInt(userInput)
    .IfSome(number => Console.WriteLine($"Number is: {number}"))
    .ElseDo(error => Console.WriteLine("Invalid number format"));
```

### HttpToolkit
Safe HTTP operations:
```csharp
using var client = new HttpClient();
await client.TryGetStringAsync("https://api.example.com/data")
    .IfSome(response => ProcessApiResponse(response))
    .ElseDo(error => Console.WriteLine($"API call failed: {error.Message}"));
```

### CollectionToolkit
Safe collection access:
```csharp
dictionary.TryGetValue("key")
    .IfSome(value => Console.WriteLine($"Found: {value}"))
    .ElseDo(error => Console.WriteLine("Key not found"));

list.TryGetAt(index)
    .IfSome(item => ProcessItem(item))
    .ElseDo(error => Console.WriteLine("Index out of range"));
```

## Installation

Install via NuGet Package Manager:
```
Install-Package FluentCoder.Maybe.Toolkit
```

Or via .NET CLI:
```
dotnet add package FluentCoder.Maybe.Toolkit
```

## Error Types

The toolkit provides specialized error types for different operations:

- **JsonError**: JSON serialization/deserialization failures
- **FileError**: File I/O operations
- **ParseError**: Parsing operations
- **HttpError**: HTTP request operations  
- **CollectionError**: Collection access operations

Each error type includes the original exception and contextual information to help with debugging.

## Usage Examples

See the [demo program](Maybe.Toolkit.Demo/Program.cs) for comprehensive usage examples of all toolkit components.

## Why Use Maybe.Toolkit?

1. **Eliminates Exception Handling**: No more try-catch blocks for common operations
2. **Fluent API**: Chain operations naturally with `IfSome`, `ElseDo`, `Select`, etc.
3. **Functional Style**: Embrace functional programming patterns in .NET
4. **Type Safety**: Compile-time guarantees that errors are handled
5. **Consistent Error Handling**: Unified approach across different operation types
6. **Composable**: Easy to combine with other Maybe operations

## Contributing

This toolkit is part of the Maybe library ecosystem. Contributions are welcome! Please see the main [Maybe repository](https://github.com/lucafabbri/Maybe) for contribution guidelines.

## License

MIT License - see the [LICENSE](../LICENSE) file for details.