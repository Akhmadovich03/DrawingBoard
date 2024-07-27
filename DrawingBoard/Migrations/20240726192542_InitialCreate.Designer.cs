﻿// <auto-generated />
using DrawingBoard.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DrawingBoard.Migrations
{
    [DbContext(typeof(BoardDbContext))]
    [Migration("20240726192542_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DrawingBoard.Models.Board", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Boards");
                });

            modelBuilder.Entity("DrawingBoard.Models.DrawingElement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BoardId")
                        .HasColumnType("int");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("EndX")
                        .HasColumnType("real");

                    b.Property<float>("EndY")
                        .HasColumnType("real");

                    b.Property<int>("LineWidth")
                        .HasColumnType("int");

                    b.Property<float>("StartX")
                        .HasColumnType("real");

                    b.Property<float>("StartY")
                        .HasColumnType("real");

                    b.Property<string>("Tool")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.ToTable("DrawingElements");
                });

            modelBuilder.Entity("DrawingBoard.Models.DrawingElement", b =>
                {
                    b.HasOne("DrawingBoard.Models.Board", "Board")
                        .WithMany("DrawingElements")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Board");
                });

            modelBuilder.Entity("DrawingBoard.Models.Board", b =>
                {
                    b.Navigation("DrawingElements");
                });
#pragma warning restore 612, 618
        }
    }
}