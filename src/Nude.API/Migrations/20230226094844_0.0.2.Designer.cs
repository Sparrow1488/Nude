﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Nude.Data.Infrastructure.Contexts;

#nullable disable

namespace Nude.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230226094844_0.0.2")]
    partial class _002
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Nude.Models.Authors.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("Nude.Models.Mangas.Manga", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Likes")
                        .HasColumnType("integer");

                    b.Property<int>("OriginUrlId")
                        .HasColumnType("integer");

                    b.Property<int>("SourceId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("OriginUrlId");

                    b.HasIndex("SourceId");

                    b.ToTable("Mangas");
                });

            modelBuilder.Entity("Nude.Models.Mangas.MangaImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MangaId")
                        .HasColumnType("integer");

                    b.Property<int>("UrlId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MangaId");

                    b.HasIndex("UrlId");

                    b.ToTable("MangaImages");
                });

            modelBuilder.Entity("Nude.Models.Requests.ParsingRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<string>("UniqueId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ParsingRequests");
                });

            modelBuilder.Entity("Nude.Models.Sources.Source", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("Nude.Models.Tags.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("MangaId")
                        .HasColumnType("integer");

                    b.Property<string>("NormalizeValue")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MangaId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Nude.Models.Urls.Url", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Urls");
                });

            modelBuilder.Entity("Nude.Models.Mangas.Manga", b =>
                {
                    b.HasOne("Nude.Models.Authors.Author", "Author")
                        .WithMany("Mangas")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nude.Models.Urls.Url", "OriginUrl")
                        .WithMany()
                        .HasForeignKey("OriginUrlId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nude.Models.Sources.Source", "Source")
                        .WithMany("Mangas")
                        .HasForeignKey("SourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("OriginUrl");

                    b.Navigation("Source");
                });

            modelBuilder.Entity("Nude.Models.Mangas.MangaImage", b =>
                {
                    b.HasOne("Nude.Models.Mangas.Manga", "Manga")
                        .WithMany("Images")
                        .HasForeignKey("MangaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nude.Models.Urls.Url", "Url")
                        .WithMany()
                        .HasForeignKey("UrlId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manga");

                    b.Navigation("Url");
                });

            modelBuilder.Entity("Nude.Models.Tags.Tag", b =>
                {
                    b.HasOne("Nude.Models.Mangas.Manga", null)
                        .WithMany("Tags")
                        .HasForeignKey("MangaId");
                });

            modelBuilder.Entity("Nude.Models.Authors.Author", b =>
                {
                    b.Navigation("Mangas");
                });

            modelBuilder.Entity("Nude.Models.Mangas.Manga", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("Tags");
                });

            modelBuilder.Entity("Nude.Models.Sources.Source", b =>
                {
                    b.Navigation("Mangas");
                });
#pragma warning restore 612, 618
        }
    }
}
