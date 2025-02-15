﻿// <auto-generated />
using System;
using EnterpriseAssignment.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EnterpriseAssignment.Migrations
{
    [DbContext(typeof(trivia_dbContext))]
    [Migration("20220417144753_CreateHasPicture")]
    partial class CreateHasPicture
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
                    b.Property<int>("IdAnswer")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AnswerString")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<int?>("IdCategory")
                        .HasColumnType("int");

                    b.Property<int?>("IdQuestion")
                        .HasColumnType("int");

                    b.Property<int?>("IdSession")
                        .HasColumnType("int");

                    b.HasKey("IdAnswer")
                        .HasName("PRIMARY");

                    b.ToTable("answer");
                });

            modelBuilder.Entity("EnterpriseAssignment.DatabaseContext.Category", b =>
                {
                    b.Property<int>("IdCategory")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CategoryName")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.HasKey("IdCategory")
                        .HasName("PRIMARY");

                    b.ToTable("category");
                });

            modelBuilder.Entity("EnterpriseAssignment.DatabaseContext.Question", b =>
                {
                    b.Property<int>("IdQuestion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AnswerA")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("AnswerB")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CorrectAnswer")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<bool>("HasPicture")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Hint")
                        .HasColumnType("longtext");

                    b.Property<int?>("IdCategory")
                        .HasColumnType("int");

                    b.Property<float>("Latitude")
                        .HasColumnType("float");

                    b.Property<float>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Prompt")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("hasLocation")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("IdQuestion")
                        .HasName("PRIMARY");

                    b.ToTable("question");
                });

            modelBuilder.Entity("EnterpriseAssignment.DatabaseContext.Session", b =>
                {
                    b.Property<int>("IdSession")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AppId")
                        .HasColumnType("longtext");

                    b.Property<int>("CurrentQuestionIndex")
                        .HasColumnType("int");

                    b.Property<bool>("EndSession")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("IdCategory")
                        .HasColumnType("int");

                    b.Property<float>("Latitude")
                        .HasColumnType("float");

                    b.Property<float>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .HasMaxLength(45)
                        .HasColumnType("varchar(45)");

                    b.Property<int>("QuestionCount")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.HasKey("IdSession")
                        .HasName("PRIMARY");

                    b.ToTable("session");
                });

            modelBuilder.Entity("EnterpriseAssignment.DatabaseContext.Sessionquestion", b =>
                {
                    b.Property<int>("IdSessionQuestion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("IdQuestion")
                        .HasColumnType("int");

                    b.Property<int?>("IdSession")
                        .HasColumnType("int");

                    b.Property<bool>("LocationHintUsed")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("int");

                    b.Property<bool>("TextHintUsed")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("IdSessionQuestion")
                        .HasName("PRIMARY");

                    b.ToTable("sessionquestion");
                });
#pragma warning restore 612, 618
        }
    }
}
