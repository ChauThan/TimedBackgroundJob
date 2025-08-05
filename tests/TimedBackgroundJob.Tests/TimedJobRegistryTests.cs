namespace TimedBackgroundJob.Tests
{
    public class TimedJobRegistryTests
    {
        [Fact]
        public void CanRegisterAndRetrieveJob()
        {
            // Arrange
            var options = new TimedJobOptions { Interval = TimeSpan.FromMilliseconds(100) };
            var registrations = new[] { new TypeJobRegistration(typeof(CustomJob), options) };
            var registry = new TimedJobRegistry(registrations);

            // Act
            var jobs = registry.Registrations;

            // Assert
            Assert.Contains(jobs, r => r is TypeJobRegistration tjr && tjr.JobType == typeof(CustomJob));
        }
    }
}
