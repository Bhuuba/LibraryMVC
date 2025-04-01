using Microsoft.EntityFrameworkCore;
using LibraryDomain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace LibraryInfrastructure
{
    public class SocialNetworkContext : IdentityDbContext<User>
    {
        public SocialNetworkContext(DbContextOptions<SocialNetworkContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Friendship> Friendships { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // ======= Coment
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Coments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // ======= Friendship
            modelBuilder.Entity<Friendship>(entity =>
            {
                entity.Property(e => e.Status)
                    .HasMaxLength(50);

                entity.HasOne(d => d.User1)
                    .WithMany(p => p.FriendshipUser1s)
                    .HasForeignKey(d => d.User1Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User2)
                    .WithMany(p => p.FriendshipUser2s)
                    .HasForeignKey(d => d.User2Id)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // ======= Group
            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(50);
            });

            // ======= Post
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            // ======= Usergroup
            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.HasOne(d => d.Group)
                    .WithMany(p => p.UserGroups)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserGroups)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}
