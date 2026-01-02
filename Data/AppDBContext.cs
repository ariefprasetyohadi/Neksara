using Microsoft.EntityFrameworkCore;
using Neksara.Models;


namespace Neksara.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<TopicView> TopicViews { get; set; }
        public DbSet<SearchLog> SearchLogs { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ===== USERS =====
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(e => e.UserId);

                entity.Property(e => e.Username)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Password)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.PhotoUrl)
                      .HasMaxLength(255);

                entity.Property(e => e.Role)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");
            });

            // ===== CATEGORIES =====
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");

                entity.HasKey(e => e.CategoriesId);

                entity.Property(e => e.CategoryName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Description)
                      .HasMaxLength(255);

                entity.Property(e => e.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");
            });

            // ===== TOPICS =====
            modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("Topics");

                entity.HasKey(e => e.TopicId);

                entity.Property(e => e.TopicName)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(e => e.Body)
                      .IsRequired();

                entity.Property(e => e.PictTopic)
                      .HasMaxLength(255);

                entity.Property(e => e.VideoUrl)
                      .HasMaxLength(255);

                entity.Property(e => e.ViewCount)
                      .HasDefaultValue(0);

                entity.Property(e => e.IsDeleted)
                      .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.Category)
                      .WithMany(c => c.Topics)
                      .HasForeignKey(e => e.IdCategory)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ===== TOPIC VIEW =====
            modelBuilder.Entity<TopicView>(entity =>
            {
                entity.ToTable("TopicView");

                entity.HasKey(e => e.TopicViewId);

                entity.Property(e => e.ViewAt)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.User)
                      .WithMany(u => u.TopicViews)
                      .HasForeignKey(e => e.IdUser);

                entity.HasOne(e => e.Topic)
                      .WithMany(t => t.TopicViews)
                      .HasForeignKey(e => e.IdTopic);
            });

            // ===== SEARCH LOG =====
            modelBuilder.Entity<SearchLog>(entity =>
            {
                entity.ToTable("SearchLogs");

                entity.HasKey(e => e.SearchLogId);

                entity.Property(e => e.Keyword)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.SearchedAt)
                      .HasDefaultValueSql("GETDATE()");
            });

            // ===== FEEDBACK =====
            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.ToTable("Feedbacks");

                entity.HasKey(e => e.FeedbackId);

                entity.Property(e => e.TargetType)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(e => e.Description)
                      .IsRequired()
                      .HasMaxLength(500);

                entity.Property(e => e.Rating)
                      .HasDefaultValue(0);

                entity.Property(e => e.IsApproved)
                      .HasDefaultValue(false);

                entity.Property(e => e.IsVisible)
                      .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");

                entity.HasOne(e => e.User)
                      .WithMany(u => u.Feedbacks)
                      .HasForeignKey(e => e.IdUser);
            });
        }
    }
}