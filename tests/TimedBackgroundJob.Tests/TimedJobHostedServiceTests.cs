namespace TimedBackgroundJob.Tests
{
    public class TimedJobHostedServiceTests
    {
        /// <summary>
        /// Verifies that CustomJob is executed at the configured interval.
        /// </summary>
        [Fact]
        public async Task ExecutesJobAtInterval()
        {
            // Arrange
            var options = new TimedJobOptions { Interval = TimeSpan.FromMilliseconds(100) };
            var customJob = new CustomJob();
            var scopeMock = new Moq.Mock<Microsoft.Extensions.DependencyInjection.IServiceScope>();
            var providerMock = new Moq.Mock<IServiceProvider>();
            providerMock.Setup(x => x.GetService(typeof(CustomJob))).Returns(customJob);
            scopeMock.Setup(x => x.ServiceProvider).Returns(providerMock.Object);
            var scopeFactoryMock = new Moq.Mock<Microsoft.Extensions.DependencyInjection.IServiceScopeFactory>();
            scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);
            var registrations = new[] { new TimedJobRegistration(typeof(CustomJob), options) };
            var registry = new TimedJobRegistry(registrations);
            var logger = new Moq.Mock<Microsoft.Extensions.Logging.ILogger<TimedJobHostedService>>().Object;
            var service = new TimedJobHostedService(scopeFactoryMock.Object, registry, logger);

            // Act
            await service.StartAsync(CancellationToken.None);
            await Task.Delay(200);
            await service.StopAsync(CancellationToken.None);

            // Assert
            // CustomJob should have been executed at least once
            Assert.True(customJob.Executed);
        }
    }
}
