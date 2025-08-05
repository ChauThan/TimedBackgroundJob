using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using TimedBackgroundJob;

namespace TimedBackgroundJob.Tests
{
    public class DelegateJobTests
    {
        [Fact]
        public async Task DelegateJob_IsExecuted()
        {
            // Arrange
            var tcs = new TaskCompletionSource<bool>();
            var options = new TimedJobOptions { Interval = TimeSpan.FromMilliseconds(50) };
            var delegateJob = new DelegateJobRegistration(async sp =>
            {
                tcs.SetResult(true);
                await Task.CompletedTask;
            }, options);
            var registry = new TimedJobRegistry(new[] { delegateJob });
            var serviceProviderMock = new Mock<IServiceProvider>();
            var scopeMock = new Mock<IServiceScope>();
            scopeMock.Setup(x => x.ServiceProvider).Returns(serviceProviderMock.Object);
            var scopeFactoryMock = new Mock<IServiceScopeFactory>();
            scopeFactoryMock.Setup(x => x.CreateScope()).Returns(scopeMock.Object);
            var logger = new Mock<ILogger<TimedJobHostedService>>().Object;
            var service = new TimedJobHostedService(scopeFactoryMock.Object, registry, logger);

            // Act
            await service.StartAsync(CancellationToken.None);
            var signaled = await Task.WhenAny(tcs.Task, Task.Delay(500));
            await service.StopAsync(CancellationToken.None);

            // Assert
            Assert.True(tcs.Task.IsCompleted, "Delegate job was not executed.");
        }
    }
}
