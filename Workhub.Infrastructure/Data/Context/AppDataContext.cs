using Microsoft.EntityFrameworkCore;
using Workhub.Domain.Entities;

namespace Workhub.Infrastructure.Data.Context;

public class AppDataContext : DbContext
{
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Job> Jobs { get; set; }
    public DbSet<ChatPost> ChatPosts { get; set; }

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
        modelBuilder.Entity<ChatPost>().OwnsMany(c => c.Replys, r =>
        {
            r.HasKey("Id");
            r.Property<string>("Id").IsRequired();
            r.Property<string>("Message").IsRequired();
            r.Property<string>("FromId").IsRequired();
            r.Property<DateTime>("CreatedOn").IsRequired();
        });


    }
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    // This method will not be used since the options are provided through the constructor
    //    optionsBuilder.UseSqlServer("Data Source=SQL5110.site4now.net;Initial Catalog=db_a7a91c_workhub;User Id=db_a7a91c_workhub_admin;Password=Kachukwu11");
    //}
}
