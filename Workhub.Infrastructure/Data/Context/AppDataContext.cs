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
        //    modelBuilder.Entity<ChatPost>().HasMany(c => c.Replys)
        //.WithOne() // Assuming there's no explicit navigation property on Reply back to ChatPost
        //.HasForeignKey("ChatPostId") // Assuming there's a foreign key property in Reply referencing ChatPost
        //.IsRequired(); // Depending on your requirements, you might need to specify if the relationship is required or optional

        //    modelBuilder.Entity<Reply>()
        //        .Property(r => r.Message)
        //        .IsRequired();

        //    modelBuilder.Entity<Reply>()
        //        .Property(r => r.FromId)
        //        .IsRequired();

        //    modelBuilder.Entity<Reply>()
        //        .Property(r => r.CreatedOn)
        //        .IsRequired();

        //modelBuilder.Entity<ChatPost>().OwnsMany(c => c.Replys, r =>
        //{
        //    r.HasKey("Id");
        //    r.Property<string>("Id").IsRequired();
        //    r.Property<string>("Message").IsRequired();
        //    r.Property<string>("FromId").IsRequired();
        //    r.Property<DateTime>("CreatedOn").IsRequired();
        //});


    }
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    // This method will not be used since the options are provided through the constructor
    //    optionsBuilder.UseSqlServer("Data Source=SQL5110.site4now.net;Initial Catalog=db_a7a91c_workhub;User Id=db_a7a91c_workhub_admin;Password=Kachukwu11");
    //}
}
