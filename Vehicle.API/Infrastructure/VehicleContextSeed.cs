using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using Vehicle.Infrastructure;

namespace Vehicle.API.Infrastructure
{
    public class VehicleContextSeed
    {
        public async Task SeedAsync(VehicleContext context, ILogger<VehicleContextSeed> logger)
        {
            var policy = CreatePolicy(logger, nameof(VehicleContextSeed));

            await policy.ExecuteAsync(() => {
                using (context)
                {
                    context.Database.Migrate();
                }

                return Task.CompletedTask;
            });
        }

        private AsyncRetryPolicy CreatePolicy(ILogger<VehicleContextSeed> logger, string prefix, int retries = 3)
        {
            return Policy.Handle<SqlException>().
                WaitAndRetryAsync(
                    retryCount: retries,
                    sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
                    onRetry: (exception, timeSpan, retry, ctx) =>
                    {
                        logger.LogWarning(exception, "[{prefix}] Error seeding database (attempt {retry} of {retries})", prefix, retry, retries);
                    }
                );
        }
    }
}
