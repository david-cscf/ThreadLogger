using Microsoft.Extensions.Configuration;
using ThreadLoggerLibrary.Logic;

namespace ThreadLogger;
public class App(IThreadManager writer, IConfiguration config)
{
    private readonly IThreadManager _threadManager = writer;
    private readonly IConfiguration _config = config;

    public void Run()
    {
        string? path = _config.GetValue<string>("OutputFile:Path");

        // Create output file
        _threadManager.InitiateFileAccess(path!);
        // Write thread logs
        _threadManager.WriteThreadLogs(10, 10);

        Console.WriteLine("Press enter to close the program");
        Console.ReadLine();

    }
}
