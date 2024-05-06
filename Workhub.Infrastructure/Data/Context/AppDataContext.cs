using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Workhub.Domain.Entities;

namespace Workhub.Infrastructure.Data.Context;

public class AppDataContext : IdentityDbContext<GlobalUser>
{
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<ChatPost> ChatPosts { get; set; }
    public DbSet<GlobalUser> GlobalUsers { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<SubHistory> SubHistorys { get; set; }

    public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Profile>().OwnsOne(p => p.Subscribe, s =>
        {
            s.Property<DateTime>(nameof(Subscribe.SubscribeOn)).HasColumnName(nameof(Subscribe.SubscribeOn));
            s.Property<DateTime>(nameof(Subscribe.ExpireOn)).HasColumnName(nameof(Subscribe.ExpireOn));
            s.Property<bool>(nameof(Subscribe.IsSubscribed)).HasColumnName(nameof(Subscribe.IsSubscribed));
        });
        modelBuilder.Entity<Profile>().OwnsOne(p => p.VendorProfile, v =>
        {
            v.Property<string>(nameof(VendorProfile.Image1)).HasColumnName(nameof(VendorProfile.Image1));
            v.Property<string>(nameof(VendorProfile.Image2)).HasColumnName(nameof(VendorProfile.Image2));
            v.Property<string>(nameof(VendorProfile.Description)).HasColumnName(nameof(VendorProfile.Description));
            v.Property<string>(nameof(VendorProfile.Instagram)).HasColumnName(nameof(VendorProfile.Instagram));
        });
        modelBuilder.Entity<Subscription>().HasData(
        new Subscription
        {
            IsEnabled = false,
            AmountInDollars = 1.00,
            AmountInNaira = 1300.0 // For example
        });
    }
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    // This method will not be used since the options are provided through the constructor
    //    optionsBuilder.UseSqlServer("Data Source=SQL5110.site4now.net;Initial Catalog=db_a7a91c_workhub;User Id=db_a7a91c_workhub_admin;Password=Kachukwu11");
    //}
}
