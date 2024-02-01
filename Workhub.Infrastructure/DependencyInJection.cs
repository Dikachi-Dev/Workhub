using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Workhub.Application.Interfaces.JWT;
using Workhub.Application.Interfaces.Logger;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Infrastructure.GlobalLogger;
using Workhub.Infrastructure.JWTToken;
using Workhub.Infrastructure.Persistance;

namespace Workhub.Infrastructure;

public static class DependencyInJection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IBuyerProfileRepository, BuyerProfileRepository>();
        services.AddScoped<ISellerProfileRepository, SellerProfileRepository>();
        services.AddScoped<IJWTGenerator, JwtTokenGenerator>();

        // Register Serilog.ILogger
        services.AddSingleton<Serilog.ILogger>(provider =>
        {
            // Configure and create your Serilog logger instance here
            return new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        });

        // Register ISeriLogger
        services.AddScoped<ISeriLogger, SeriLogger>();
        //services.AddDbContext < AppDataContext >
        return services;
    }
}
