using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Workhub.Infrastructure.Data.Context;

public class AppDataContextFactory : IDesignTimeDbContextFactory<AppDataContext>
{
    public AppDataContext CreateDbContext(string[] args)
    {
        // Get the directory of the startup project (Workhub.Api)
        // When running from solution root, we need to point to the Api project
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "Workhub.Api");
        if (!Directory.Exists(basePath))
        {
            // Fallback to current directory if Workhub.Api not found (e.g. running from inside Workhub.Api)
            basePath = Directory.GetCurrentDirectory();
        }

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var builder = new DbContextOptionsBuilder<AppDataContext>();
        var connectionString = configuration.GetConnectionString("AppDataContext");

        // Fallback connection string if configuration fails
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = "Server=localhost;Port=5432;Database=WorkHubAPi;User Id=postgres;Password=admin;Include Error Detail=true";
        }

        builder.UseNpgsql(connectionString, o => o.UseNetTopologySuite());

        return new AppDataContext(builder.Options);
    }
}
