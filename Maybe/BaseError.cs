using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Maybe;

/// <summary>
/// Represents the base class for all specialized errors in the system.
/// It provides common properties and functionality for error handling.
/// </summary>
public abstract class BaseError : IError
{
    private BaseError? _innerError;

    /// <summary>
    /// Gets the inner error, if any, that caused this error.
    /// This is used to preserve the error chain.
    /// </summary>
    public BaseError? InnerError => _innerError;

    /// <inheritdoc/>
    public abstract OutcomeType Type { get; protected set; }

    /// <inheritdoc/>
    public abstract string Code { get; protected set; }

    /// <inheritdoc/>
    public abstract string Message { get; protected set; }

    /// <inheritdoc/>
    public int TimeStamp { get; } = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseError"/> class.
    /// </summary>
    public BaseError()
    {
    }

    /// <summary>
    /// Sets the inner error for this error instance.
    /// </summary>
    /// <param name="innerError">The error that is the cause of the current error.</param>
    public void SetInnerError(BaseError innerError)
    {
        _innerError = innerError;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj?.GetType() == GetType() && obj is BaseError other && Equals(other);
    }

    /// <inheritdoc/>
    public bool Equals(IError other)
    {
        if (other is null) return false;
        return Type == other.Type && Code == other.Code;
    }

    /// <summary>
    /// Returns a simple string representation of the error.
    /// </summary>
    /// <returns>A string in the format "[Type] Code: Message".</returns>
    public override string ToString()
    {
        return $"[{Type}] {Code}: {Message}";
    }

    /// <summary>
    /// Returns a full, formatted string representation of the entire error chain.
    /// The layout uses aligned columns and wraps long messages for readability, suitable for detailed logging.
    /// </summary>
    /// <returns>The formatted string of the entire error chain.</returns>
    public string ToFullString()
    {
        // 1. Collect all errors and their hierarchy depth
        var allErrors = new List<(int depth, BaseError error)>();
        CollectErrors(this, 0, allErrors);
        if (!allErrors.Any()) return string.Empty;

        // 2. Define layout constants
        const int totalWidth = 120;
        const int timestampColumnWidth = 24; // Width for "[YYYY-MM-DD HH:MM:SS]  "

        // 3. Calculate dynamic column widths based on content
        int maxDepth = allErrors.Max(e => e.depth);
        int maxTypeWidth = allErrors.Max(e => $"[{e.error.Type}]".Length) + 1 + (maxDepth * 2); // +1 for space
        int maxCodeWidth = allErrors.Max(e => e.error.Code.Length) + 3; // +3 for spaces around

        var sb = new StringBuilder();

        foreach (var (depth, error) in allErrors)
        {
            // 4. Prepare parts for the current error line
            string indent = new string(' ', depth * 2);
            string typePart = $"{indent}[{error.Type}]".PadRight(maxTypeWidth);
            string codePart = error.Code.PadRight(maxCodeWidth);
            string timestampPart = $"[{DateTimeOffset.FromUnixTimeSeconds(error.TimeStamp).ToLocalTime():yyyy-MM-dd HH:mm:ss}]".PadRight(timestampColumnWidth);

            // 5. Calculate available width for the message and wrap it
            int messageColumnStart = maxTypeWidth + maxCodeWidth + timestampColumnWidth;
            int messageMaxWidth = totalWidth - messageColumnStart;
            var messageLines = WrapText(error.Message, messageMaxWidth);

            // 6. Append the first line containing all columns
            string firstLine = messageLines.FirstOrDefault() ?? string.Empty;
            sb.AppendLine($"{typePart}{codePart}{timestampPart}{firstLine}");

            // 7. Append subsequent lines from the wrapped message, indented correctly
            string messageIndent = new string(' ', messageColumnStart);
            foreach (var line in messageLines.Skip(1))
            {
                sb.AppendLine($"{messageIndent}{line}");
            }
        }

        return sb.ToString().TrimEnd();
    }

    /// <summary>
    /// Recursively collects all inner errors into a list with their depth.
    /// </summary>
    private void CollectErrors(BaseError? current, int depth, List<(int depth, BaseError error)> list)
    {
        if (current == null) return;
        list.Add((depth, current));
        CollectErrors(current.InnerError, depth + 1, list);
    }

    /// <summary>
    /// Wraps a string into multiple lines based on a maximum width.
    /// </summary>
    private List<string> WrapText(string text, int maxWidth)
    {
        if (string.IsNullOrWhiteSpace(text) || maxWidth <= 0) return [""];

        var lines = new List<string>();
        var words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        var currentLine = new StringBuilder();

        foreach (var word in words)
        {
            if (currentLine.Length > 0 && currentLine.Length + word.Length + 1 > maxWidth)
            {
                lines.Add(currentLine.ToString());
                currentLine.Clear();
            }

            if (currentLine.Length > 0)
                currentLine.Append(" ");

            currentLine.Append(word);
        }

        if (currentLine.Length > 0)
            lines.Add(currentLine.ToString());

        return lines.Any() ? lines : [""];
    }

    public override int GetHashCode()
    {
#if NETSTANDARD2_0
                int hash = 17;
                hash = hash * 31 + Type.GetHashCode();
                hash = hash * 31 + (Code?.GetHashCode() ?? 0);
                hash = hash * 31 + TimeStamp.GetHashCode();
                return hash;
#else
        return HashCode.Combine(Type, Code, TimeStamp);
#endif
    }
}

