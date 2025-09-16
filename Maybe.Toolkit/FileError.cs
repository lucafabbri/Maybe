using Maybe;

namespace Maybe.Toolkit;

/// <summary>
/// Error for file I/O operations.
/// </summary>
public class FileError : FailureError
{
    public override OutcomeType Type => OutcomeType.Failure;
    public override string Code => "File.IOError";
    public override string Message => "File operation failed.";

    /// <summary>
    /// The original exception that caused the file error.
    /// </summary>
    public Exception? OriginalException { get; private set; }

    /// <summary>
    /// The file path that was being accessed when the error occurred.
    /// </summary>
    public string? FilePath { get; private set; }

    public FileError() { }

    public FileError(Exception originalException, string? filePath = null, string? customMessage = null)
    {
        OriginalException = originalException;
        FilePath = filePath;
        if (customMessage != null)
        {
            Message = customMessage;
        }
    }
}