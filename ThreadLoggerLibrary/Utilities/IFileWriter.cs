namespace ThreadLoggerLibrary.Utilities;

/// <summary>
/// Wraps I/O data access to an output file and handles file creation.
/// </summary>
/// <remarks>
/// Requires to call to <see cref="CreateOutputFile(string)"></see> before writing to the file. 
/// </remarks>
public interface IFileWriter: IDisposable
{
    /// <summary>
    /// Writes a new line on the output file.
    /// </summary>
    /// <param name="text">path where the out.txt file will be created</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the output file had not been created <see cref="CreateOutputFile(string)"/>
    /// </exception>
    void WriteLine(string text);
    /// <summary>
    /// Creates output file.
    /// </summary>
    /// <param name="path">Path for the output file</param>
    void CreateOutputFile(string path);
    /// <summary>
    /// Dumps the StreamWriter buffer contents into the output file.
    /// </summary>
    void Flush();
}