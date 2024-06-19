﻿// <auto-generated />
using System;
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
    [Migration("20240619074708_TPH_Blogs")]
    partial class TPH_Blogs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("WebDatabaseProject.BlogBase", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ParentBlogId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ParentBlogId");

                    b.ToTable("Blogs");

                    b.HasDiscriminator<string>("Discriminator").HasValue("BlogBase");
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

                    b.HasIndex("BlogId", "Key")
                        .IsUnique();

                    b.ToTable("BlogConfigs");
                });

            modelBuilder.Entity("WebDatabaseProject.ShopBlog", b =>
                {
                    b.HasBaseType("WebDatabaseProject.BlogBase");

                    b.Property<string>("ShopUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("ShopBlog");
                });

            modelBuilder.Entity("WebDatabaseProject.TechBlog", b =>
                {
                    b.HasBaseType("WebDatabaseProject.BlogBase");

                    b.Property<string>("GithubUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("TechBlog");
                });

            modelBuilder.Entity("WebDatabaseProject.BlogBase", b =>
                {
                    b.HasOne("WebDatabaseProject.BlogBase", "ParentBlog")
                        .WithMany("ChildBlogs")
                        .HasForeignKey("ParentBlogId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("ParentBlog");
                });

            modelBuilder.Entity("WebDatabaseProject.BlogConfig", b =>
                {
                    b.HasOne("WebDatabaseProject.BlogBase", null)
                        .WithMany("Configs")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebDatabaseProject.BlogBase", b =>
                {
                    b.Navigation("ChildBlogs");

                    b.Navigation("Configs");
                });
#pragma warning restore 612, 618
        }
    }
}