using Microsoft.EntityFrameworkCore;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Api.Configurations;

public static class DatabaseSetup
{
    public static void AddDatabaseSetup(this IServiceCollection services)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                             .Build();

        var db = configuration.GetConnectionString("AppDataContext");

        if (services is null)
            throw new ArgumentNullException(nameof(services));

        services.AddDbContext<AppDataContext>(
            options => options.UseSqlServer(db)
        );
    }
}
