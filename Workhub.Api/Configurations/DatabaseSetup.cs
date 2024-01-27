using Microsoft.EntityFrameworkCore;
using Workhub.Infrastructure.Data.Context;

namespace Workhub.Api.Configurations;

public static class DatabaseSetup
{
    public static void AddDatabaseSetup(this IServiceCollection services)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                             .AddJsonFile("appsettings.json").Build();
        var db = configuration.GetSection("ConnectionStrings:AppDataContext").Value;


        if (services is null) throw new ArgumentNullException(nameof(services));

        services.AddDbContext<AppDataContext>(
                  options => options.UseSqlServer(db)
            );
    }
}
