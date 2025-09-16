using Maybe;

namespace Maybe.Toolkit;

/// <summary>
/// Provides Maybe-based wrapper methods for System.IO.File operations.
/// </summary>
public static class FileToolkit
{
    /// <summary>
    /// Attempts to read all text from a file, returning a Maybe result.
    /// </summary>
    /// <param name="path">The path to the file to read.</param>
    /// <returns>A Maybe containing the file contents or a FileError.</returns>
    public static Maybe<string, FileError> TryReadAllText(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return new FileError(new ArgumentException("File path cannot be null or empty"), path, "File path cannot be null or empty");
        }

        try
        {
            var content = File.ReadAllText(path);
            return Maybe<string, FileError>.Some(content);
        }
        catch (FileNotFoundException ex)
        {
            return new FileError(ex, path, $"File not found: {path}");
        }
        catch (DirectoryNotFoundException ex)
        {
            return new FileError(ex, path, $"Directory not found for file: {path}");
        }
        catch (UnauthorizedAccessException ex)
        {
            return new FileError(ex, path, $"Access denied to file: {path}");
        }
        catch (IOException ex)
        {
            return new FileError(ex, path, $"I/O error reading file: {path}");
        }
        catch (Exception ex)
        {
            return new FileError(ex, path, $"Unexpected error reading file: {path}");
        }
    }

    /// <summary>
    /// Attempts to read all text from a file with the specified encoding, returning a Maybe result.
    /// </summary>
    /// <param name="path">The path to the file to read.</param>
    /// <param name="encoding">The encoding to use when reading the file.</param>
    /// <returns>A Maybe containing the file contents or a FileError.</returns>
    public static Maybe<string, FileError> TryReadAllText(string path, System.Text.Encoding encoding)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return new FileError(new ArgumentException("File path cannot be null or empty"), path, "File path cannot be null or empty");
        }

        if (encoding == null)
        {
            return new FileError(new ArgumentNullException(nameof(encoding)), path, "Encoding cannot be null");
        }

        try
        {
            var content = File.ReadAllText(path, encoding);
            return Maybe<string, FileError>.Some(content);
        }
        catch (FileNotFoundException ex)
        {
            return new FileError(ex, path, $"File not found: {path}");
        }
        catch (DirectoryNotFoundException ex)
        {
            return new FileError(ex, path, $"Directory not found for file: {path}");
        }
        catch (UnauthorizedAccessException ex)
        {
            return new FileError(ex, path, $"Access denied to file: {path}");
        }
        catch (IOException ex)
        {
            return new FileError(ex, path, $"I/O error reading file: {path}");
        }
        catch (Exception ex)
        {
            return new FileError(ex, path, $"Unexpected error reading file: {path}");
        }
    }

    /// <summary>
    /// Attempts to read all bytes from a file, returning a Maybe result.
    /// </summary>
    /// <param name="path">The path to the file to read.</param>
    /// <returns>A Maybe containing the file bytes or a FileError.</returns>
    public static Maybe<byte[], FileError> TryReadAllBytes(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return new FileError(new ArgumentException("File path cannot be null or empty"), path, "File path cannot be null or empty");
        }

        try
        {
            var bytes = File.ReadAllBytes(path);
            return Maybe<byte[], FileError>.Some(bytes);
        }
        catch (FileNotFoundException ex)
        {
            return new FileError(ex, path, $"File not found: {path}");
        }
        catch (DirectoryNotFoundException ex)
        {
            return new FileError(ex, path, $"Directory not found for file: {path}");
        }
        catch (UnauthorizedAccessException ex)
        {
            return new FileError(ex, path, $"Access denied to file: {path}");
        }
        catch (IOException ex)
        {
            return new FileError(ex, path, $"I/O error reading file: {path}");
        }
        catch (Exception ex)
        {
            return new FileError(ex, path, $"Unexpected error reading file: {path}");
        }
    }

    /// <summary>
    /// Attempts to write all text to a file, returning a Maybe result.
    /// </summary>
    /// <param name="path">The path to the file to write.</param>
    /// <param name="contents">The string to write to the file.</param>
    /// <returns>A Maybe indicating success or a FileError.</returns>
    public static Maybe<Unit, FileError> TryWriteAllText(string path, string contents)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return new FileError(new ArgumentException("File path cannot be null or empty"), path, "File path cannot be null or empty");
        }

        if (contents == null)
        {
            return new FileError(new ArgumentNullException(nameof(contents)), path, "Contents cannot be null");
        }

        try
        {
            File.WriteAllText(path, contents);
            return Maybe<Unit, FileError>.Some(Unit.Value);
        }
        catch (DirectoryNotFoundException ex)
        {
            return new FileError(ex, path, $"Directory not found for file: {path}");
        }
        catch (UnauthorizedAccessException ex)
        {
            return new FileError(ex, path, $"Access denied to file: {path}");
        }
        catch (IOException ex)
        {
            return new FileError(ex, path, $"I/O error writing file: {path}");
        }
        catch (Exception ex)
        {
            return new FileError(ex, path, $"Unexpected error writing file: {path}");
        }
    }

    /// <summary>
    /// Attempts to write all bytes to a file, returning a Maybe result.
    /// </summary>
    /// <param name="path">The path to the file to write.</param>
    /// <param name="bytes">The bytes to write to the file.</param>
    /// <returns>A Maybe indicating success or a FileError.</returns>
    public static Maybe<Unit, FileError> TryWriteAllBytes(string path, byte[] bytes)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return new FileError(new ArgumentException("File path cannot be null or empty"), path, "File path cannot be null or empty");
        }

        if (bytes == null)
        {
            return new FileError(new ArgumentNullException(nameof(bytes)), path, "Bytes cannot be null");
        }

        try
        {
            File.WriteAllBytes(path, bytes);
            return Maybe<Unit, FileError>.Some(Unit.Value);
        }
        catch (DirectoryNotFoundException ex)
        {
            return new FileError(ex, path, $"Directory not found for file: {path}");
        }
        catch (UnauthorizedAccessException ex)
        {
            return new FileError(ex, path, $"Access denied to file: {path}");
        }
        catch (IOException ex)
        {
            return new FileError(ex, path, $"I/O error writing file: {path}");
        }
        catch (Exception ex)
        {
            return new FileError(ex, path, $"Unexpected error writing file: {path}");
        }
    }
}

/// <summary>
/// Represents a unit value - a type with only one value, used to indicate success without a meaningful return value.
/// </summary>
public readonly struct Unit
{
    /// <summary>
    /// The singleton value of Unit.
    /// </summary>
    public static readonly Unit Value = new();
    
    /// <summary>
    /// Returns a string representation of the Unit value.
    /// </summary>
    public override string ToString() => "()";
}