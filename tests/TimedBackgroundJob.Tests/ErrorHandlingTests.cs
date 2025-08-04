using Microsoft.Extensions.Logging;
namespace TimedBackgroundJob.Tests
{
    public class ErrorHandlingTests
    {
        /// <summary>
        /// Verifies that errors in jobs are logged and do not crash the service.
        /// </summary>
        [Fact]
        public async Task HandlesJobErrorGracefully()
        {
            // Arrange: Set up registry with a failing job and a mock logger to capture error logs.
            var registry = new TimedJobRegistry(new List<TimedJobRegistration>());
            var options = new TimedJobOptions { Interval = TimeSpan.FromMilliseconds(50) };
            registry.Add(new TimedJobRegistration(typeof(FailingJob), options));
            var scopeFactory = new Moq.Mock<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>().Object;
            var loggerMock = new Moq.Mock<ILogger<TimedJobHostedService>>();
            var service = new TimedJobHostedService(scopeFactory, registry, loggerMock.Object);

            // Act: Start and stop the service, allowing time for the job to run and fail.
            await service.StartAsync(CancellationToken.None);
            await Task.Delay(100);
            await service.StopAsync(CancellationToken.None);

            // Assert: Verify that the logger was called with an error log for the failing job.
            loggerMock.Verify(
                x => x.Log(
                    Moq.It.Is<LogLevel>(level => level == LogLevel.Error),
                    Moq.It.IsAny<EventId>(),
                    Moq.It.Is<Moq.It.IsAnyType>((v, t) =>
                        v != null &&
                        v.ToString() != null &&
                        v.ToString().Contains("Error executing timed job") &&
                        v.ToString().Contains("FailingJob")
                    ),
                    Moq.It.IsAny<Exception>(),
                    Moq.It.IsAny<Func<Moq.It.IsAnyType, Exception?, string>>()
                ),
                Moq.Times.AtLeastOnce,
                "Expected error log for FailingJob was not found."
            );
            // The test passes if the error is logged and no unhandled exceptions occur.
        }
    }
}
