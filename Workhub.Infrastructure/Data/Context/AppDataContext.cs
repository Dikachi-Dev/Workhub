

using Microsoft.EntityFrameworkCore;
using Workhub.Domain.Entities;

namespace Workhub.Infrastructure.Data.Context;

public class AppDataContext : DbContext
{
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Job> Jobs { get; set; }

    public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // This method will not be used since the options are provided through the constructor
        optionsBuilder.UseSqlServer("Data Source=SQL5110.site4now.net;Initial Catalog=db_a7a91c_workhub;User Id=db_a7a91c_workhub_admin;Password=Kachukwu11");
    }
}
