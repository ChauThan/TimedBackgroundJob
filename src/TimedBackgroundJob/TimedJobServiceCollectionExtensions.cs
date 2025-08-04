using Microsoft.Extensions.DependencyInjection;

namespace TimedBackgroundJob
{
    public static class TimedJobServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a timed background job with the specified options.
        /// </summary>
        /// <typeparam name="TJob">The job type implementing ITimedJob.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="configureOptions">Options configuration delegate.</param>
        public static IServiceCollection AddTimedJob<TJob>(
            this IServiceCollection services,
            Action<TimedJobOptions> configureOptions)
            where TJob : class, ITimedJob
        {
            var options = new TimedJobOptions();
            configureOptions?.Invoke(options);
            services.AddSingleton<ITimedJob, TJob>();
            services.AddSingleton<TimedJobRegistration>(sp => new TimedJobRegistration(typeof(TJob), options));

            // Ensure registry and hosted service are registered only once
            var alreadyRegistered = services.Any(sd => sd.ServiceType == typeof(TimedJobRegistry));
            if (!alreadyRegistered)
            {
                services.AddSingleton<TimedJobRegistry>(sp =>
                {
                    var registrations = sp.GetServices<TimedJobRegistration>();
                    return new TimedJobRegistry(registrations);
                });
                services.AddHostedService<TimedJobHostedService>();
            }
            return services;
        }
    }
}
