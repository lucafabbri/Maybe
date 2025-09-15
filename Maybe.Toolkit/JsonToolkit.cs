using System.Text.Json;
using Maybe;

namespace Maybe.Toolkit;

/// <summary>
/// Provides Maybe-based wrapper methods for System.Text.Json.JsonSerializer operations.
/// </summary>
public static class JsonToolkit
{
    /// <summary>
    /// Attempts to deserialize the JSON string to the specified type, returning a Maybe result.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="json">The JSON string to deserialize.</param>
    /// <param name="options">Optional JsonSerializerOptions.</param>
    /// <returns>A Maybe containing the deserialized object or a JsonError.</returns>
    public static Maybe<T, JsonError> TryDeserialize<T>(string json, JsonSerializerOptions? options = null)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return new JsonError(new ArgumentException("JSON string cannot be null or empty"), "JSON string cannot be null or empty");
        }

        try
        {
            var result = JsonSerializer.Deserialize<T>(json, options);
            if (result == null && !typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) == null)
            {
                return new JsonError(new InvalidOperationException("Deserialization resulted in null"), "Deserialization resulted in null");
            }
            return Maybe<T, JsonError>.Some(result!);
        }
        catch (JsonException ex)
        {
            return new JsonError(ex, $"Failed to deserialize JSON to {typeof(T).Name}");
        }
        catch (ArgumentNullException ex)
        {
            return new JsonError(ex, "JSON input was null");
        }
        catch (NotSupportedException ex)
        {
            return new JsonError(ex, $"Type {typeof(T).Name} is not supported for JSON deserialization");
        }
        catch (Exception ex)
        {
            return new JsonError(ex, $"Unexpected error during JSON deserialization to {typeof(T).Name}");
        }
    }

    /// <summary>
    /// Attempts to deserialize the UTF-8 JSON bytes to the specified type, returning a Maybe result.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="utf8Json">The UTF-8 JSON bytes to deserialize.</param>
    /// <param name="options">Optional JsonSerializerOptions.</param>
    /// <returns>A Maybe containing the deserialized object or a JsonError.</returns>
    public static Maybe<T, JsonError> TryDeserialize<T>(ReadOnlySpan<byte> utf8Json, JsonSerializerOptions? options = null)
    {
        if (utf8Json.Length == 0)
        {
            return new JsonError(new ArgumentException("UTF-8 JSON bytes cannot be empty"), "UTF-8 JSON bytes cannot be empty");
        }

        try
        {
            var result = JsonSerializer.Deserialize<T>(utf8Json, options);
            if (result == null && !typeof(T).IsValueType && Nullable.GetUnderlyingType(typeof(T)) == null)
            {
                return new JsonError(new InvalidOperationException("Deserialization resulted in null"), "Deserialization resulted in null");
            }
            return Maybe<T, JsonError>.Some(result!);
        }
        catch (JsonException ex)
        {
            return new JsonError(ex, $"Failed to deserialize UTF-8 JSON to {typeof(T).Name}");
        }
        catch (ArgumentNullException ex)
        {
            return new JsonError(ex, "UTF-8 JSON input was null");
        }
        catch (NotSupportedException ex)
        {
            return new JsonError(ex, $"Type {typeof(T).Name} is not supported for JSON deserialization");
        }
        catch (Exception ex)
        {
            return new JsonError(ex, $"Unexpected error during UTF-8 JSON deserialization to {typeof(T).Name}");
        }
    }

    /// <summary>
    /// Attempts to serialize the object to a JSON string, returning a Maybe result.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="value">The object to serialize.</param>
    /// <param name="options">Optional JsonSerializerOptions.</param>
    /// <returns>A Maybe containing the JSON string or a JsonError.</returns>
    public static Maybe<string, JsonError> TrySerialize<T>(T value, JsonSerializerOptions? options = null)
    {
        try
        {
            var result = JsonSerializer.Serialize(value, options);
            return Maybe<string, JsonError>.Some(result);
        }
        catch (JsonException ex)
        {
            return new JsonError(ex, $"Failed to serialize {typeof(T).Name} to JSON");
        }
        catch (NotSupportedException ex)
        {
            return new JsonError(ex, $"Type {typeof(T).Name} is not supported for JSON serialization");
        }
        catch (Exception ex)
        {
            return new JsonError(ex, $"Unexpected error during JSON serialization of {typeof(T).Name}");
        }
    }
}