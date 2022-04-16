﻿// <auto-generated />
using System;
using EnterpriseAssignment.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EnterpriseAssignment.Migrations
{
    [DbContext(typeof(quiz_dbContext))]
    [Migration("20220414235740_ChangeColumnName2")]
    partial class ChangeColumnName2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.16");

            modelBuilder.Entity("EnterpriseAssignment.DatabaseContext.Answer", b =>
                {
                    b.Property<int>("Idanswer")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idanswer");

                    b.Property<string>("AnswerCharacter")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("answerCharacter");

                    b.Property<int?>("Idcategory")
                        .HasColumnType("int")
                        .HasColumnName("idcategory");

                    b.Property<int?>("Idquestion")
                        .HasColumnType("int")
                        .HasColumnName("idquestion");

                    b.Property<int?>("Idsession")
                        .HasColumnType("int")
                        .HasColumnName("idsession");

                    b.HasKey("Idanswer")
                        .HasName("PRIMARY");

                    b.ToTable("answer");
                });

            modelBuilder.Entity("EnterpriseAssignment.DatabaseContext.Category", b =>
                {
                    b.Property<int>("Idcategory")
                        .HasColumnType("int")
                        .HasColumnName("idcategory");

                    b.Property<string>("CategoryName")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.HasKey("Idcategory")
                        .HasName("PRIMARY");

                    b.ToTable("category");
                });

            modelBuilder.Entity("EnterpriseAssignment.DatabaseContext.Question", b =>
                {
                    b.Property<int>("Idquestion")
                        .HasColumnType("int")
                        .HasColumnName("idquestion");

                    b.Property<string>("AnswerA")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("answerA");

                    b.Property<string>("AnswerB")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("answerB");

                    b.Property<string>("CorrectAnswer")
                        .HasMaxLength(1)
                        .HasColumnType("varchar(1)")
                        .HasColumnName("correct_answer");

                    b.Property<int?>("Idcategory")
                        .HasColumnType("int")
                        .HasColumnName("idcategory");

                    b.Property<string>("Prompt")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("prompt");

                    b.HasKey("Idquestion")
                        .HasName("PRIMARY");

                    b.HasIndex("Idcategory");

                    b.ToTable("question");
                });

            modelBuilder.Entity("EnterpriseAssignment.DatabaseContext.Session", b =>
                {
                    b.Property<int>("Idsession")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idsession");

                    b.Property<string>("CurrentQuestionIndex")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("current_question");

                    b.Property<int?>("Idcategory")
                        .HasColumnType("int")
                        .HasColumnName("idcategory");

                    b.Property<string>("Name")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)")
                        .HasColumnName("name");

                    b.Property<int?>("Score")
                        .HasColumnType("int")
                        .HasColumnName("score");

                    b.HasKey("Idsession")
                        .HasName("PRIMARY");

                    b.ToTable("session");
                });

            modelBuilder.Entity("EnterpriseAssignment.DatabaseContext.SessionQuestion", b =>
                {
                    b.Property<int>("Idsessionquestion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("idsessionquestion");

                    b.Property<int?>("Idquestion")
                        .HasColumnType("int")
                        .HasColumnName("idquestion");

                    b.Property<int?>("Idsession")
                        .HasColumnType("int")
                        .HasColumnName("idsession");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("int")
                        .HasColumnName("orderIndex");

                    b.HasKey("Idsessionquestion")
                        .HasName("PRIMARY");

                    b.ToTable("sessionQuestion");
                });

            modelBuilder.Entity("EnterpriseAssignment.DatabaseContext.Question", b =>
                {
                    b.HasOne("EnterpriseAssignment.DatabaseContext.Category", "Category")
                        .WithMany("QuestionList")
                        .HasForeignKey("Idcategory");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("EnterpriseAssignment.DatabaseContext.Category", b =>
                {
                    b.Navigation("QuestionList");
                });
#pragma warning restore 612, 618
        }
    }
}
