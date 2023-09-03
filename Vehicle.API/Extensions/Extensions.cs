using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Vehicle.Infrastructure;

namespace Vehicle.API.Extensions
{
    internal static class Extensions
    {
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            static void ConfigureSqlOptions(SqlServerDbContextOptionsBuilder sqlOptions)
            {
                sqlOptions.MigrationsAssembly(typeof(Program).Assembly.FullName);

                // Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 

                sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
            };

            services.AddDbContext<VehicleContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("VehiclesDB"), ConfigureSqlOptions);
            });

            return services;
        }
    }
}
