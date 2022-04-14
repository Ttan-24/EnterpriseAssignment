using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EnterpriseAssignment.DatabaseContext
{
    public partial class quiz_dbContext : DbContext
    {
        public quiz_dbContext()
        {
        }

        public quiz_dbContext(DbContextOptions<quiz_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }

        public virtual DbSet<SessionQuestion> SessionQuestions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci");

            modelBuilder.Entity<Answer>(entity =>
            {
                entity.HasKey(e => e.Idanswer)
                    .HasName("PRIMARY");

                entity.ToTable("answer");

                entity.Property(e => e.Idanswer).HasColumnName("idanswer");

                entity.Property(e => e.AnswerCharacter)
                    .HasMaxLength(45)
                    .HasColumnName("answerCharacter");

                entity.Property(e => e.Idcategory).HasColumnName("idcategory");

                entity.Property(e => e.Idquestion).HasColumnName("idquestion");

                entity.Property(e => e.Iduser).HasColumnName("iduser");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Idcategory)
                    .HasName("PRIMARY");

                entity.ToTable("category");

                entity.Property(e => e.Idcategory)
                    .ValueGeneratedNever()
                    .HasColumnName("idcategory");

                entity.Property(e => e.CategoryName)
                    .HasMaxLength(45);

                entity.HasMany(e => e.QuestionList)
                    .WithOne(q => q.Category)
                    .HasForeignKey(q => q.Idcategory);
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(e => e.Idquestion)
                    .HasName("PRIMARY");

                entity.ToTable("question");

                entity.Property(e => e.Idquestion)
                    .ValueGeneratedNever()
                    .HasColumnName("idquestion");

                entity.Property(e => e.AnswerA)
                    .HasMaxLength(45)
                    .HasColumnName("answerA");

                entity.Property(e => e.AnswerB)
                    .HasMaxLength(45)
                    .HasColumnName("answerB");

                entity.Property(e => e.CorrectAnswer)
                    .HasMaxLength(1)
                    .HasColumnName("correct_answer");

                entity.Property(e => e.Idcategory).HasColumnName("idcategory");

                entity.Property(e => e.Prompt)
                    .HasMaxLength(255)
                    .HasColumnName("prompt");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.HasKey(e => e.Iduser)
                    .HasName("PRIMARY");

                entity.ToTable("session");

                entity.Property(e => e.Iduser).HasColumnName("iduser");

                entity.Property(e => e.CurrentQuestion)
                    .HasMaxLength(45)
                    .HasColumnName("current_question");

                entity.Property(e => e.Idcategory).HasColumnName("idcategory");

                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.Score).HasColumnName("score");
            });

            modelBuilder.Entity<SessionQuestion>(entity =>
            {
                entity.HasKey(e => e.Idsessionquestion)
                    .HasName("PRIMARY");

                entity.ToTable("sessionQuestion");

                entity.Property(e => e.Iduser).HasColumnName("iduser");

                entity.Property(e => e.Idquestion).HasColumnName("idquestion");

                entity.Property(e => e.OrderIndex).HasColumnName("orderIndex");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
