using Microsoft.Extensions.Logging;
using Moq;
using ThreadLoggerLibrary.Logic;
using ThreadLoggerLibrary.Utilities;

namespace ThreadLoggerTests;

public class ThreadManagerTests
{
    // Uses a mock for the FileWriter class so a file is not actually created and repeatedly written
    // for testing. Especially important if live testing is enabled.
    [Fact]
    public void LogThreadData_Complete()
    {
        // Arrange
        var service = new Mock<IFileWriter>();
        service.Setup(x => x.WriteLine(It.IsAny<string>()));
        var logger = new Mock<ILogger<ThreadManager>>();

        var sut = new ThreadManager(service.Object, logger.Object);

        // Act
        sut.WriteThreadLogs(10, 10);

        // Assert
        service.Verify(x => x.WriteLine(It.IsAny<string>()), Times.Exactly(100));
        Assert.Equal(101, sut.CurrentLine); 
    }
}