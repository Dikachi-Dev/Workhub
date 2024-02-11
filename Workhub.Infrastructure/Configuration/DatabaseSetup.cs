//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Workhub.Infrastructure.Data.Context;


//namespace Workhub.Infrastructure.Configuration;

//public static class DatabaseSetup
//{
//    public static void AddDatabaseSetup(this IServiceCollection services)
//    {
//        IConfigurationRoot configuration = new ConfigurationBuilder()
//                             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//                             .Build();

//        var db = configuration.GetConnectionString("AppDataContext");

//        if (services is null)
//            throw new ArgumentNullException(nameof(services));

//        services.AddDbContext<AppDataContext>(
//            options => options.UseSqlServer(db)
//        );
//    }
//}
