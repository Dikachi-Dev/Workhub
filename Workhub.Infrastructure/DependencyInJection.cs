using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Workhub.Application.Interfaces.JWT;
using Workhub.Application.Interfaces.Logger;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;
using Workhub.Domain.Entities;
using Workhub.Infrastructure.Data.Context;
using Workhub.Infrastructure.GlobalLogger;
using Workhub.Infrastructure.JWTToken;
using Workhub.Infrastructure.Persistance;
using Workhub.Infrastructure.Services;

namespace Workhub.Infrastructure;

public static class DependencyInJection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        //IConfigurationRoot configuration = new ConfigurationBuilder()
        //                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //                   .Build();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IJWTGenerator, JwtTokenGenerator>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IChatPostRepository, ChatPostRepository>();
        services.AddScoped<ICloseProx, CloseProx>();
        services.AddScoped<ICheckVerify, CheckVerify>();
        services.AddScoped<IEmailSender, EmailSender>();
        //services.AddIdentity<GlobalUser, IdentityRole>(option =>
        //option.User.RequireUniqueEmail = true
        //)
        //    .AddEntityFrameworkStores<AppDataContext>().AddDefaultTokenProviders();
        services.AddIdentity<GlobalUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;

            // Configure password options
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6; // Minimum password length
        })
.AddEntityFrameworkStores<AppDataContext>()
.AddDefaultTokenProviders();
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
        services.AddDbContext<AppDataContext>();
        return services;
    }
}
