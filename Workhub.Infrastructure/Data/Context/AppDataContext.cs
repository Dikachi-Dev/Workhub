using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer("ConnectionString");
    }
}