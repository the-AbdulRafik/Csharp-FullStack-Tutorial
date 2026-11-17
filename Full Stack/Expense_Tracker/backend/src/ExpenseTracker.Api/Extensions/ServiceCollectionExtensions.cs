using ExpenseTracker.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContext(
        this IServiceCollection services)
    {
        services.AddDbContext<ExpenseTrackerDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("ExpenseTracker");
            options.UseSqlServer(connectionString);
        });

        return services;
    }
}
