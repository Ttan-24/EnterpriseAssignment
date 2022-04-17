using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EnterpriseAssignment.DatabaseContext
{
    public partial class trivia_dbContext : DbContext
    {
        public trivia_dbContext()
        {
        }

        public trivia_dbContext(DbContextOptions<trivia_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Sessionquestion> Sessionquestions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;user id=root;password=root;database=trivia_db", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.HasKey(e => e.IdAnswer)
                    .HasName("PRIMARY");

                entity.ToTable("answer");

                entity.Property(e => e.AnswerString).HasMaxLength(45);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.IdCategory)
                    .HasName("PRIMARY");

                entity.ToTable("category");

                entity.Property(e => e.CategoryName).HasMaxLength(45);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(e => e.IdQuestion)
                    .HasName("PRIMARY");

                entity.ToTable("question");

                entity.Property(e => e.AnswerA).HasMaxLength(255);

                entity.Property(e => e.AnswerB).HasMaxLength(255);

                entity.Property(e => e.CorrectAnswer).HasMaxLength(45);

                entity.Property(e => e.Prompt).HasMaxLength(255);
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.IdSession)
                    .HasName("PRIMARY");

                entity.ToTable("session");

                entity.Property(e => e.Name).HasMaxLength(45);
            });

            modelBuilder.Entity<Sessionquestion>(entity =>
            {
                entity.HasKey(e => e.IdSessionQuestion)
                    .HasName("PRIMARY");

                entity.ToTable("sessionquestion");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
