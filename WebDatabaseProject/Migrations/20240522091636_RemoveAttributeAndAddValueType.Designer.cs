﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebDatabaseProject;

#nullable disable

namespace WebDatabaseProject.Migrations
{
    [DbContext(typeof(BlogContext))]
    [Migration("20240522091636_RemoveAttributeAndAddValueType")]
    partial class RemoveAttributeAndAddValueType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebDatabaseProject.Blog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("WebDatabaseProject.BlogConfig", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<long>("BlogId")
                        .HasColumnType("bigint");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ValueTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ValueTypeId");

                    b.HasIndex("BlogId", "Key")
                        .IsUnique();

                    b.ToTable("BlogConfigs");
                });

            modelBuilder.Entity("WebDatabaseProject.EAV.ValueTypeBase", b =>
                {
                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TypeId");

                    b.ToTable("ValueTypes");

                    b.HasDiscriminator<int>("TypeId");
                });

            modelBuilder.Entity("WebDatabaseProject.EAV.BoolValueType", b =>
                {
                    b.HasBaseType("WebDatabaseProject.EAV.ValueTypeBase");

                    b.HasDiscriminator().HasValue(5);

                    b.HasData(
                        new
                        {
                            TypeId = 5,
                            Name = "True/False"
                        });
                });

            modelBuilder.Entity("WebDatabaseProject.EAV.DateTimeValueType", b =>
                {
                    b.HasBaseType("WebDatabaseProject.EAV.ValueTypeBase");

                    b.HasDiscriminator().HasValue(4);

                    b.HasData(
                        new
                        {
                            TypeId = 4,
                            Name = "DateTime"
                        });
                });

            modelBuilder.Entity("WebDatabaseProject.EAV.DecimalValueType", b =>
                {
                    b.HasBaseType("WebDatabaseProject.EAV.ValueTypeBase");

                    b.HasDiscriminator().HasValue(3);

                    b.HasData(
                        new
                        {
                            TypeId = 3,
                            Name = "Real Number"
                        });
                });

            modelBuilder.Entity("WebDatabaseProject.EAV.IntegerValueType", b =>
                {
                    b.HasBaseType("WebDatabaseProject.EAV.ValueTypeBase");

                    b.HasDiscriminator().HasValue(2);

                    b.HasData(
                        new
                        {
                            TypeId = 2,
                            Name = "Number"
                        });
                });

            modelBuilder.Entity("WebDatabaseProject.EAV.StringValueType", b =>
                {
                    b.HasBaseType("WebDatabaseProject.EAV.ValueTypeBase");

                    b.HasDiscriminator().HasValue(1);

                    b.HasData(
                        new
                        {
                            TypeId = 1,
                            Name = "String"
                        });
                });

            modelBuilder.Entity("WebDatabaseProject.EAV.UserValueType", b =>
                {
                    b.HasBaseType("WebDatabaseProject.EAV.ValueTypeBase");

                    b.HasDiscriminator().HasValue(6);

                    b.HasData(
                        new
                        {
                            TypeId = 6,
                            Name = "User"
                        });
                });

            modelBuilder.Entity("WebDatabaseProject.BlogConfig", b =>
                {
                    b.HasOne("WebDatabaseProject.Blog", null)
                        .WithMany("Configs")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebDatabaseProject.EAV.ValueTypeBase", "ValueType")
                        .WithMany()
                        .HasForeignKey("ValueTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ValueType");
                });

            modelBuilder.Entity("WebDatabaseProject.Blog", b =>
                {
                    b.Navigation("Configs");
                });
#pragma warning restore 612, 618
        }
    }
}