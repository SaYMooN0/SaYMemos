using Microsoft.EntityFrameworkCore;
using SaYMemos.Models.data.entities.memos;
using SaYMemos.Models.data.entities.users;
using SaYMemos.Models.data.entities.comments;

public class MemoDbContext : DbContext
{
    public DbSet<LoginInfo> LoginInfos { get; private set; }
    public DbSet<User> Users { get; private set; }
    public DbSet<UserLinks> UserLinks { get; private set; }
    public DbSet<UserAdditionalInfo> UserAdditionalInfos { get; private set; }
    public DbSet<UserToConfirm> UsersToConfirm { get; private set; }
    public DbSet<MemoTag> MemoTags { get; private set; }

    public DbSet<Memo> Memos { get; private set; }
    public DbSet<MemoLike> Likes { get; private set; }
    public DbSet<Comment> Comments { get; private set; }
    public DbSet<CommentRating> CommentRatings { get; private set; }

    public MemoDbContext(DbContextOptions<MemoDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LoginInfo>().HasKey(x => x.Id);
        modelBuilder.Entity<User>().HasKey(x => x.Id);
        modelBuilder.Entity<UserLinks>().HasKey(x => x.Id);
        modelBuilder.Entity<UserAdditionalInfo>().HasKey(x => x.Id);
        modelBuilder.Entity<UserToConfirm>().HasKey(x => x.Id);

        modelBuilder.Entity<Memo>().HasKey(x => x.id);
        modelBuilder.Entity<MemoTag>().HasKey(x => x.Id);


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

        modelBuilder.Entity<Memo>()
            .HasOne(m => m.Author)
            .WithMany(u => u.PostedMemos)
            .HasForeignKey(m => m.authorId);

        modelBuilder.Entity<Memo>()
            .HasMany(m => m.Tags)
            .WithMany(t => t.Memos);

        modelBuilder.Entity<MemoLike>()
               .HasOne(like => like.User)
               .WithMany(user => user.Likes)
               .HasForeignKey(like => like.userId);

        modelBuilder.Entity<MemoLike>()
            .HasOne(like => like.Memo)
            .WithMany(memo => memo.Likes)
            .HasForeignKey(like => like.memoGuid);






        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Memo)
            .WithMany(m => m.Comments)
            .HasForeignKey(c => c.memoId);
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Author)
            .WithMany(user => user.Comments)
            .HasForeignKey(c => c.authorId);
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.ParentComment)
            .WithMany(c => c.ChildComments)
            .HasForeignKey(c => c.parentCommentId);


        modelBuilder.Entity<CommentRating>()
            .HasOne(cr => cr.Comment)
            .WithMany(c => c.Ratings)
            .HasForeignKey(cr => cr.commentId);
        modelBuilder.Entity<CommentRating>()
            .HasOne(cr => cr.User)
            .WithMany(u=>u.CommentRatings)
            .HasForeignKey(cr => cr.userId);

    }
}
