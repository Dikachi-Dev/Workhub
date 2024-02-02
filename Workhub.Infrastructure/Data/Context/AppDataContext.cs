using Microsoft.EntityFrameworkCore;
using Workhub.Domain.Entities;

namespace Workhub.Infrastructure.Data.Context;

public class AppDataContext : DbContext
{
    public DbSet<Profile> Profiles { get; set; }

    public AppDataContext()
    {
    }

    public AppDataContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    base.OnConfiguring(optionsBuilder);
    //    optionsBuilder.UseSqlServer("ConnectionString");
    //}
}