using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SaYMemos.Models.data.entities.users;

public class MemoDbContext : DbContext
{
    public DbSet<LoginInfo> LoginInfos { get; private set; }
    public DbSet<User> Users { get; private set; }
    public DbSet<UserLinks> UserLinks { get; private set; }
    public DbSet<UserAdditionalInfo> UserAdditionalInfos { get; private set; } 
    public DbSet<UserToConfirm> UsersToConfirm { get; private set; }

    public MemoDbContext(DbContextOptions<MemoDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LoginInfo>().HasKey(x => x.Id);
        modelBuilder.Entity<User>().HasKey(x => x.Id);
        modelBuilder.Entity<UserLinks>().HasKey(x => x.Id);
        modelBuilder.Entity<UserAdditionalInfo>().HasKey(x => x.Id);
        modelBuilder.Entity<UserToConfirm>().HasKey(x => x.Id);

        modelBuilder.Entity<User>()
            .HasOne(u => u.AdditionalInfo)
            .WithOne()
            .HasForeignKey<User>(u => u.AdditionalInfoId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.LoginInfo)
            .WithOne()
            .HasForeignKey<User>(u => u.LoginInfoId);

        modelBuilder.Entity<User>()
            .HasOne(u => u.UserLinks)
            .WithOne()
            .HasForeignKey<User>(u => u.UserLinksId);
    }
}
