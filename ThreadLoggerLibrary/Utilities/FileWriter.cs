
namespace ThreadLoggerLibrary.Utilities;
/// <inheritdoc cref="IFileWriter"/>
public class FileWriter : IFileWriter
{
    private StreamWriter? _streamWriter;

    /// <inheritdoc/>
    public void WriteLine(string text)
    {
        if (_streamWriter is null)
            throw new InvalidOperationException("Output file should be created first");
        
        _streamWriter.WriteLine(text);
    }

    /// <inheritdoc/>
    public void CreateOutputFile(string path)
    {
        // Check log location is accessible
        string? dirPath = Path.GetDirectoryName(path);
        
        _ = Directory.CreateDirectory(dirPath!);
        Directory.SetCurrentDirectory(dirPath!);

        _streamWriter = new StreamWriter(path);

    }

    /// <inheritdoc/>
    public void Flush()
    {
        if (_streamWriter is null)
            throw new InvalidOperationException("Output file should be created first");

        _streamWriter.Flush();
    }

    public void Dispose()
    {
        _streamWriter?.Dispose();
        GC.SuppressFinalize(this);
    }
}
