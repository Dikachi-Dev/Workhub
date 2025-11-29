using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
using Hangfire;
using Hangfire.PostgreSql;
using Workhub.Infrastructure.BackgroundJobs;
using Workhub.Infrastructure.Data.Migrations;

namespace Workhub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // ========== Database Configuration ==========
        ConfigureDatabase(services, configuration);

        // ========== Repository Services (Scoped) ==========
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<IChatPostRepository, ChatPostRepository>();

        // ========== Business Services (Scoped) ==========
        services.AddScoped<IJWTGenerator, JwtTokenGenerator>();
        services.AddScoped<ICloseProx, CloseProx>();
        services.AddScoped<IotpGenerator, OtpGenerator>();
        services.AddScoped<ICheckVerify, CheckVerify>();
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<INotificationSender, NotificationSender>();

        // ========== Identity Configuration ==========
        ConfigureIdentity(services);

        // ========== Logging Configuration ==========
        ConfigureLogging(services);

        // ========== Firebase Configuration ==========
        ConfigureFirebase();

        // ========== Hangfire Configuration ==========
        ConfigureHangfire(services, configuration);

        return services;
    }

    private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDataContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("AppDataContext");
            options.UseNpgsql(connectionString, o => o.UseNetTopologySuite());
        });
    }

    private static void ConfigureIdentity(IServiceCollection services)
    {
        services.AddIdentity<GlobalUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;

            // Password requirements
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;
        })
        .AddEntityFrameworkStores<AppDataContext>()
        .AddDefaultTokenProviders();
    }

    private static void ConfigureLogging(IServiceCollection services)
    {
        // Register Serilog (Singleton for performance)
        services.AddSingleton<Serilog.ILogger>(provider =>
        {
            return new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        });

        // Register custom logger wrapper (Scoped)
        services.AddScoped<ISeriLogger, SeriLogger>();
    }

    private static void ConfigureFirebase()
    {
        try
        {
            var credential = GoogleCredential.FromFile("firebase.json");
            FirebaseApp.Create(new AppOptions
            {
                Credential = credential
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Firebase initialization failed: {ex.Message}");
        }
    }

    private static void ConfigureHangfire(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AppDataContext");
        
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(options => 
                options.UseNpgsqlConnection(connectionString)));

        services.AddHangfireServer();

        // Register background job services
        services.AddScoped<OtpCleanupJob>();
    }


    /// <summary>
    /// Automatically applies pending database migrations at application startup
    /// </summary>
    public static async Task TryAddMigration(this IServiceProvider service)
    {
        var dbContext = service.GetRequiredService<AppDataContext>();

        if (dbContext.Database.IsInMemory())
            return;

        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            try
            {
                await dbContext.Database.MigrateAsync();
            }
            catch (Exception ex) when (ex.Message.Contains("extension \"postgis\" is not available") || ex.InnerException?.Message.Contains("extension \"postgis\" is not available") == true)
            {
                throw new Exception("CRITICAL: PostGIS extension is missing on the PostgreSQL server. Please install PostGIS to continue.", ex);
            }
        }

        // Apply manual PostGIS migration
        await PostGISMigrationHelper.ApplyPostGISMigration(dbContext);
    }
}
