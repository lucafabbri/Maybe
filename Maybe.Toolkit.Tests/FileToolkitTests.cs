using FluentAssertions;
using Maybe;
using Maybe.Toolkit;

namespace Maybe.Toolkit.Tests;

public class FileToolkitTests
{
    private readonly string _tempDirectory;

    public FileToolkitTests()
    {
        _tempDirectory = Path.GetTempPath();
    }

    [Fact]
    public void TryReadAllText_WithExistingFile_ReturnsSuccess()
    {
        // Arrange
        var tempFile = Path.Combine(_tempDirectory, $"test_{Guid.NewGuid()}.txt");
        var expectedContent = "Hello, World!";
        File.WriteAllText(tempFile, expectedContent);

        try
        {
            // Act
            var result = FileToolkit.TryReadAllText(tempFile);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ValueOrThrow().Should().Be(expectedContent);
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void TryReadAllText_WithNonExistentFile_ReturnsFileError()
    {
        // Arrange
        var nonExistentFile = Path.Combine(_tempDirectory, $"nonexistent_{Guid.NewGuid()}.txt");

        // Act
        var result = FileToolkit.TryReadAllText(nonExistentFile);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<FileError>();
        error.Code.Should().Be("File.IOError");
        error.FilePath.Should().Be(nonExistentFile);
        error.OriginalException.Should().BeOfType<FileNotFoundException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void TryReadAllText_WithInvalidPath_ReturnsFileError(string invalidPath)
    {
        // Act
        var result = FileToolkit.TryReadAllText(invalidPath);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<FileError>();
        error.FilePath.Should().Be(invalidPath);
    }

    [Fact]
    public void TryReadAllBytes_WithExistingFile_ReturnsSuccess()
    {
        // Arrange
        var tempFile = Path.Combine(_tempDirectory, $"test_{Guid.NewGuid()}.bin");
        var expectedBytes = new byte[] { 1, 2, 3, 4, 5 };
        File.WriteAllBytes(tempFile, expectedBytes);

        try
        {
            // Act
            var result = FileToolkit.TryReadAllBytes(tempFile);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ValueOrThrow().Should().BeEquivalentTo(expectedBytes);
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void TryWriteAllText_WithValidPath_ReturnsSuccess()
    {
        // Arrange
        var tempFile = Path.Combine(_tempDirectory, $"test_{Guid.NewGuid()}.txt");
        var content = "Test content";

        try
        {
            // Act
            var result = FileToolkit.TryWriteAllText(tempFile, content);

            // Assert
            result.IsSuccess.Should().BeTrue();
            File.Exists(tempFile).Should().BeTrue();
            File.ReadAllText(tempFile).Should().Be(content);
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void TryWriteAllText_WithNullContent_ReturnsFileError()
    {
        // Arrange
        var tempFile = Path.Combine(_tempDirectory, $"test_{Guid.NewGuid()}.txt");

        // Act
        var result = FileToolkit.TryWriteAllText(tempFile, null!);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<FileError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void TryWriteAllBytes_WithValidPath_ReturnsSuccess()
    {
        // Arrange
        var tempFile = Path.Combine(_tempDirectory, $"test_{Guid.NewGuid()}.bin");
        var bytes = new byte[] { 1, 2, 3, 4, 5 };

        try
        {
            // Act
            var result = FileToolkit.TryWriteAllBytes(tempFile, bytes);

            // Assert
            result.IsSuccess.Should().BeTrue();
            File.Exists(tempFile).Should().BeTrue();
            File.ReadAllBytes(tempFile).Should().BeEquivalentTo(bytes);
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void TryReadAllText_WithEncoding_ReturnsSuccess()
    {
        // Arrange
        var tempFile = Path.Combine(_tempDirectory, $"test_{Guid.NewGuid()}.txt");
        var expectedContent = "Hello, UTF-8!";
        var encoding = System.Text.Encoding.UTF8;
        File.WriteAllText(tempFile, expectedContent, encoding);

        try
        {
            // Act
            var result = FileToolkit.TryReadAllText(tempFile, encoding);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ValueOrThrow().Should().Be(expectedContent);
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public void TryReadAllText_WithNullEncoding_ReturnsFileError()
    {
        // Arrange
        var tempFile = Path.Combine(_tempDirectory, $"test_{Guid.NewGuid()}.txt");

        // Act
        var result = FileToolkit.TryReadAllText(tempFile, null!);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<FileError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void TryReadAllBytes_WithNonExistentFile_ReturnsFileError()
    {
        // Arrange
        var nonExistentFile = Path.Combine(_tempDirectory, $"nonexistent_{Guid.NewGuid()}.bin");

        // Act
        var result = FileToolkit.TryReadAllBytes(nonExistentFile);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<FileError>();
        error.Code.Should().Be("File.IOError");
        error.FilePath.Should().Be(nonExistentFile);
        error.OriginalException.Should().BeOfType<FileNotFoundException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void TryReadAllBytes_WithInvalidPath_ReturnsFileError(string? invalidPath)
    {
        // Act
        var result = FileToolkit.TryReadAllBytes(invalidPath!);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<FileError>();
        error.FilePath.Should().Be(invalidPath);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void TryWriteAllText_WithInvalidPath_ReturnsFileError(string? invalidPath)
    {
        // Act
        var result = FileToolkit.TryWriteAllText(invalidPath!, "content");

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<FileError>();
        error.FilePath.Should().Be(invalidPath);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void TryWriteAllBytes_WithInvalidPath_ReturnsFileError(string? invalidPath)
    {
        // Act
        var result = FileToolkit.TryWriteAllBytes(invalidPath!, new byte[] { 1, 2, 3 });

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<FileError>();
        error.FilePath.Should().Be(invalidPath);
    }

    [Fact]
    public void TryWriteAllBytes_WithNullBytes_ReturnsFileError()
    {
        // Arrange
        var tempFile = Path.Combine(_tempDirectory, $"test_{Guid.NewGuid()}.bin");

        // Act
        var result = FileToolkit.TryWriteAllBytes(tempFile, null!);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<FileError>();
        error.OriginalException.Should().BeOfType<ArgumentNullException>();
    }

    [Fact]
    public void TryWriteAllText_WithReadOnlyFile_ReturnsFileError()
    {
        // This test checks unauthorized access scenarios
        var tempFile = Path.Combine(_tempDirectory, $"readonly_{Guid.NewGuid()}.txt");
        
        try
        {
            File.WriteAllText(tempFile, "initial content");
            File.SetAttributes(tempFile, FileAttributes.ReadOnly);

            // Act
            var result = FileToolkit.TryWriteAllText(tempFile, "new content");

            // Assert
            result.IsError.Should().BeTrue();
            var error = result.ErrorOrThrow();
            error.Should().BeOfType<FileError>();
            error.OriginalException.Should().BeOfType<UnauthorizedAccessException>();
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                File.SetAttributes(tempFile, FileAttributes.Normal);
                File.Delete(tempFile);
            }
        }
    }

    [Fact]
    public void TryReadAllText_WithDirectoryPath_ReturnsFileError()
    {
        // Arrange - use a directory path instead of file path
        var directoryPath = _tempDirectory;

        // Act
        var result = FileToolkit.TryReadAllText(directoryPath);

        // Assert
        result.IsError.Should().BeTrue();
        var error = result.ErrorOrThrow();
        error.Should().BeOfType<FileError>();
        error.FilePath.Should().Be(directoryPath);
    }

    [Fact]
    public void Unit_Value_IsNotNull()
    {
        // Test the Unit struct that's used in write operations
        var unit = Unit.Value;
        unit.Should().NotBeNull();
        
        // Test that two Unit values are equal
        var unit2 = Unit.Value;
        unit.Should().Be(unit2);
    }

    [Fact]
    public void Unit_ToString_ReturnsExpectedValue()
    {
        // Test Unit.ToString() method
        var unit = Unit.Value;
        var stringValue = unit.ToString();
        stringValue.Should().NotBeNull();
    }
}