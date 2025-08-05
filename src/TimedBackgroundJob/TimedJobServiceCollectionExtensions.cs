using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
            services.AddSingleton<TJob>();
            services.AddSingleton<TimedJobRegistration>(sp => new TypeJobRegistration(typeof(TJob), options));
            EnsureRegistryAndHostedServiceRegistered(services);
            return services;
        }
        /// <summary>
        /// Registers a timed background job using a delegate for job logic.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="configureOptions">Options configuration delegate.</param>
        /// <param name="jobDelegate">Delegate containing job logic, receives IServiceProvider.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddTimedJob(
            this IServiceCollection services,
            Action<TimedJobOptions> configureOptions,
            Func<IServiceProvider, Task> jobDelegate)
        {
            var options = new TimedJobOptions();
            configureOptions?.Invoke(options);
            services.AddSingleton<TimedJobRegistration>(sp => new DelegateJobRegistration(jobDelegate, options));
            EnsureRegistryAndHostedServiceRegistered(services);
            return services;
        }
        /// <summary>
        /// Automatically discovers and registers all timed jobs decorated with <see cref="TimedJobAttribute"/>.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="assemblies">Assemblies to scan for timed jobs. If null, uses entry and referenced assemblies.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddTimedJobsFromAttributes(
            this IServiceCollection services,
            IEnumerable<System.Reflection.Assembly>? assemblies = null)
        {
            // Restrict scanning to relevant assemblies
            var scanAssemblies = assemblies?.ToList() ?? AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location)).ToList();

            var discoveredTypes = new List<Type>();
            var registrations = new List<TimedJobRegistration>();
            var errors = new List<string>();

            foreach (var assembly in scanAssemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    // Only public, non-abstract classes implementing ITimedJob
                    if (!type.IsClass || type.IsAbstract || !type.IsPublic)
                        continue;
                    if (!typeof(ITimedJob).IsAssignableFrom(type))
                        continue;
                    var attr = (TimedJobAttribute?)System.Reflection.CustomAttributeExtensions.GetCustomAttribute(type, typeof(TimedJobAttribute));
                    if (attr is null)
                        continue;

                    // Skip if job already registered manually
                    if (services.Any(sd => sd.ServiceType == type))
                        continue;

                    if (discoveredTypes.Contains(type))
                    {
                        errors.Add($"Duplicate timed job type discovered: {type.FullName}");
                        continue;
                    }
                    discoveredTypes.Add(type);

                    // Map attribute parameters to TimedJobOptions
                    var options = new TimedJobOptions
                    {
                        Interval = attr.Interval,
                        PreventOverlap = attr.PreventOverlap
                    };
                    registrations.Add(new TypeJobRegistration(type, options));
                    services.AddSingleton(type);
                }
            }

            // Register discovered jobs and their options
            foreach (var reg in registrations)
            {
                services.AddSingleton<TimedJobRegistration>(sp => reg);
            }

            EnsureRegistryAndHostedServiceRegistered(services);

            // Log errors for misconfigured or duplicate jobs
            if (errors.Count > 0)
            {
                // Use built-in logging if available, otherwise throw
                var logger = services.BuildServiceProvider().GetService<Microsoft.Extensions.Logging.ILoggerFactory>()?.CreateLogger("TimedJobDiscovery");
                foreach (var error in errors)
                {
                    if (logger != null)
                    {
                        ((ILogger)logger).LogError(error);
                    }
                }
                if (logger is null)
                {
                    throw new InvalidOperationException($"Timed job discovery errors: {string.Join(", ", errors)}");
                }
            }

            return services;
        }

        /// <summary>
        /// Ensures that TimedJobRegistry and TimedJobHostedService are registered only once in the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        private static void EnsureRegistryAndHostedServiceRegistered(IServiceCollection services)
        {
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
        }
    }
}
