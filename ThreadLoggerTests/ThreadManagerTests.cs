using Microsoft.Extensions.Logging;
using Moq;
using ThreadLoggerLibrary.Logic;
using ThreadLoggerLibrary.Utilities;

namespace ThreadLoggerTests;

public class ThreadManagerTests
{
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

    }
}