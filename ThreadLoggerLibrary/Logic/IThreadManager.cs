namespace ThreadLoggerLibrary.Logic;

/// <summary>
/// Coordinates concurrent log writes done by multiple threads to the same output file.
/// </summary>
/// <remarks>
/// Although I/O operations are good candidates for multithreading, in this example they all occur
/// over the same shared resource with no other parallel work taking place.<br/>
/// This approach is not recommended for a real case scenario and only presented here for demonstration purposes.<br/>
/// Multithreading and parallelism are different things. The worker threads created by this class are effectively runing sequentially.<br/>
/// There are no performance gains, but the opposite, the context switch and threads synchronization will have a penalty on performance. 
/// </remarks>
public interface IThreadManager : IDisposable
{
    /// <summary>
    /// Initial ckeck that the program can create the output file.<br/>
    /// Initializes it with "0, 0, timestamp".<br/>
    /// If exception is thrown, records it and passes it up the chain to stop execution.
    /// </summary>
    /// <param name="path">path where the out.txt file will be created</param>
    void InitiateFileAccess(string path);
    /// <summary>
    /// Writes to out.txt in a mutithreaded maner logging
    /// the line number, thread id and timestamp in each line.
    /// <para>
    /// Complexity analysis:<br/>
    /// Time: O(n*m)<br/>
    /// Space: O(n*m) for the StreamWriter buffer which has a size limit => ~ O(1)
    /// </para>
    /// </summary>
    /// <param name="numThreads">Number of concurrent threads to be created</param>
    /// <param name="numChecks">Number of lines per thread to be recorded</param>
    void WriteThreadLogs(int numThreads, int numChecks);
}