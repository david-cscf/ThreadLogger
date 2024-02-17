using Microsoft.Extensions.Logging;
using System.Globalization;
using ThreadLoggerLibrary.Utilities;

namespace ThreadLoggerLibrary.Logic;
/// <inheritdoc cref="IThreadManager"/>
public class ThreadManager( IFileWriter writer, ILogger<ThreadManager> log) : IThreadManager
{
    private readonly IFileWriter _writer = writer;
    private readonly ILogger<ThreadManager> _log = log;
    private readonly object _writerLock = new();
    public int CurrentLine { get; private set; } = 1;
    private Exception? _lastException;
    readonly CancellationTokenSource _cts = new();

    /// <inheritdoc/>
    public void WriteThreadLogs(int numThreads, int numChecks)
    {
        Thread[] threads = CreateThreads(numThreads, numChecks);
        foreach (var thread in threads)
        {
            thread.Join();
        }
        if (_lastException is not null)
        {
            _log.LogError(_lastException, "Error ocurred in one of the worker threads");
            throw _lastException;
        }
        _writer.Flush();
    }

    private Thread[] CreateThreads(int numThreads, int numChecks)
    {
        var threads = new Thread[numThreads];
        for (int i = 0; i < numThreads; i++)
        {
            threads[i] = new Thread(() => WriteStatus(numChecks));
            threads[i].Start();
        }
        return threads;
    }

    private void WriteStatus(int numChecks)
    {
        try
        {
            for (int j = 0; j < numChecks; j++)
            {
                // If execption ocurred exit threads
                if (_cts.Token.IsCancellationRequested) return;
                lock (_writerLock)
                {
                    var timeStamp = DateTime.Now.ToString("HH:mm:ss.ffff", CultureInfo.InvariantCulture);
                    string output = $"{CurrentLine}, {Environment.CurrentManagedThreadId}, {timeStamp}";
                    _writer.WriteLine(output);
                    _log.LogInformation("{LineCount}, {ThreadId}, {TimeStamp}",
                        CurrentLine, Environment.CurrentManagedThreadId, timeStamp);
                    CurrentLine++;
                }
            }
        }
        catch (Exception ex)
        {
            _lastException = ex;
            _cts.Cancel();
        }
    }

    /// <inheritdoc/>
    public void InitiateFileAccess(string path)
    {
        try
        {
            _writer.CreateOutputFile(path);
            var timeStamp = DateTime.Now.ToString("HH:mm:ss.ffff", CultureInfo.InvariantCulture);
            _writer.WriteLine($"0, 0 {timeStamp}");
            _log.LogInformation("0, 0, {TimeStamp}", timeStamp);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Error creating output file");
            throw;
        }
    }

    // FileWriter instance will be diposed by IoC container since it was
    // reposnsible of its creation through DI.
    // Disposes the cancellation token field that was created by this class.
    public void Dispose()
    {
        _cts?.Dispose();
        GC.SuppressFinalize(this);
    }
}
