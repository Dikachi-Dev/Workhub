using Microsoft.EntityFrameworkCore;
using Workhub.Domain.Entities;

namespace Workhub.Infrastructure.Data.Context;

public class AppDataContext : DbContext
{
    public DbSet<BuyerProfile> BuyerProfiles { get; set; }
    public DbSet<SellerProfile> SellerProfiles { get; set; }

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