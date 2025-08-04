namespace TimedBackgroundJob.Example
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.AddTimedJob<HelloWorldJob>(options =>
                    {
                        options.Interval = TimeSpan.FromSeconds(10);
                    });
                    services.AddTimedJob<TimeLoggerJob>(options =>
                    {
                        options.Interval = TimeSpan.FromSeconds(15);
                    });
                })
                .Build();

            host.Run();
        }
    }
}
