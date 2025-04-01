using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using LibraryDomain.Model;

namespace LibraryInfrastructure
{
    public partial class DblibraryContext : DbContext
    {
        public DblibraryContext(DbContextOptions<DblibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Coment> Coments { get; set; }
        public virtual DbSet<Friendship> Friendships { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Usergroup> Usergroups { get; set; }

        // Якщо Scaffold створив partial-метод, залишайте його
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning Move your connection string to appsettings.json or Secrets Manager
            => optionsBuilder.UseSqlServer("Server=DESKTOP-Q512LK2; Database=DBLibrary; Trusted_Connection=True; TrustServerCertificate=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ======= Coment
            modelBuilder.Entity<Coment>(entity =>
            {
                entity.ToTable("coments");

                // Заміна ValueGeneratedNever() на ValueGeneratedOnAdd()
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.CreateAt).HasColumnName("create_at");
                entity.Property(e => e.PostId).HasColumnName("post_id");
                entity.Property(e => e.Text).HasColumnName("text");
                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Coments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_comments_posts_post_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Coments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_comments_user_user_id");
            });

            // ======= Friendship
            modelBuilder.Entity<Friendship>(entity =>
            {
                entity.ToTable("friendship");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd() // було ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.CreateAt).HasColumnName("create_at");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");
                entity.Property(e => e.User1Id).HasColumnName("user1_id");
                entity.Property(e => e.User2Id).HasColumnName("user2_id");

                entity.HasOne(d => d.User1)
                    .WithMany(p => p.FriendshipUser1s)
                    .HasForeignKey(d => d.User1Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_friendship_user_user_id1");

                entity.HasOne(d => d.User2)
                    .WithMany(p => p.FriendshipUser2s)
                    .HasForeignKey(d => d.User2Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_friendship_user_user_id2");
            });

            // ======= Group
            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("group");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd() // було ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.CreateAt).HasColumnName("create_at");
                entity.Property(e => e.Info).HasColumnName("info");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            // ======= Post
            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("posts");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd() // було ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.CreateAt).HasColumnName("create_at");
                entity.Property(e => e.Text).HasColumnName("text");
                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // ======= User
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd() // було ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Birthdate).HasColumnName("birthdate");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .HasColumnName("status");
            });

            // ======= Usergroup
            modelBuilder.Entity<Usergroup>(entity =>
            {
                entity.ToTable("usergroup");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd() // було ValueGeneratedNever()
                    .HasColumnName("id");
                entity.Property(e => e.GroupId).HasColumnName("group_id");
                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Usergroups)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Usergroups)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // Якщо Scaffold створив partial-метод, залишайте його
            OnModelCreatingPartial(modelBuilder);
        }
    }
}
