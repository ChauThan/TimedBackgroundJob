namespace TimedBackgroundJob.Tests
{
    public class OverlapPreventionTests
    {
        /// <summary>
        /// Verifies that LongRunningJob does not overlap when PreventOverlap is true.
        /// </summary>
        [Fact]
        public async Task PreventsJobOverlap()
        {
            // Arrange
            var options = new TimedJobOptions { Interval = TimeSpan.FromMilliseconds(50), PreventOverlap = true };
            var longRunningJob = new LongRunningJob();
            var scopeMock = new Moq.Mock<Microsoft.Extensions.DependencyInjection.IServiceScope>();
            var providerMock = new Moq.Mock<IServiceProvider>();
            providerMock.Setup(x => x.GetService(typeof(LongRunningJob))).Returns(longRunningJob);
            scopeMock.Setup(x => x.ServiceProvider).Returns(providerMock.Object);
            var scopeFactoryMock = new Moq.Mock<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>();
            scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);
            var registrations = new[] { new TypeJobRegistration(typeof(LongRunningJob), options) };
            var registry = new TimedJobRegistry(registrations);
            var logger = new Moq.Mock<Microsoft.Extensions.Logging.ILogger<TimedJobHostedService>>().Object;
            var service = new TimedJobHostedService(scopeFactoryMock.Object, registry, logger);

            // Act
            await service.StartAsync(CancellationToken.None);
            await Task.Delay(200);
            await service.StopAsync(CancellationToken.None);

            // Assert
            // LongRunningJob should not overlap, so ExecutionCount should be 1
            Assert.Equal(1, longRunningJob.ExecutionCount);
        }
    }
}
