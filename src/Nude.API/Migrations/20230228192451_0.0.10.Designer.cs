﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Nude.API.Data.Contexts;

#nullable disable

namespace Nude.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230228192451_0.0.10")]
    partial class _0010
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MangaTag", b =>
                {
                    b.Property<int>("MangasId")
                        .HasColumnType("integer");

                    b.Property<int>("TagsId")
                        .HasColumnType("integer");

                    b.HasKey("MangasId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("MangaTag");
                });

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

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ExternalId")
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

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

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

                    b.Property<string>("NormalizeValue")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Nude.Models.Tickets.FeedBackInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CallbackUrl")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("FeedBackInfos");
                });

            modelBuilder.Entity("Nude.Models.Tickets.ParsingMeta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("EntityType")
                        .HasColumnType("integer");

                    b.Property<string>("SourceItemId")
                        .HasColumnType("text");

                    b.Property<string>("SourceUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TicketId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TicketId")
                        .IsUnique();

                    b.ToTable("ParsingMetas");
                });

            modelBuilder.Entity("Nude.Models.Tickets.ParsingResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("EntityId")
                        .HasColumnType("text");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StatusCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TicketId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("TicketId")
                        .IsUnique();

                    b.ToTable("ParsingResults");
                });

            modelBuilder.Entity("Nude.Models.Tickets.ParsingTicket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("ParsingTickets");
                });

            modelBuilder.Entity("Nude.Models.Tickets.Subscriber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FeedBackInfoId")
                        .HasColumnType("integer");

                    b.Property<int>("NotifyStatus")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FeedBackInfoId");

                    b.ToTable("Subscribers");
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

            modelBuilder.Entity("ParsingTicketSubscriber", b =>
                {
                    b.Property<int>("SubscribersId")
                        .HasColumnType("integer");

                    b.Property<int>("TicketsId")
                        .HasColumnType("integer");

                    b.HasKey("SubscribersId", "TicketsId");

                    b.HasIndex("TicketsId");

                    b.ToTable("ParsingTicketSubscriber");
                });

            modelBuilder.Entity("MangaTag", b =>
                {
                    b.HasOne("Nude.Models.Mangas.Manga", null)
                        .WithMany()
                        .HasForeignKey("MangasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nude.Models.Tags.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

            modelBuilder.Entity("Nude.Models.Tickets.ParsingMeta", b =>
                {
                    b.HasOne("Nude.Models.Tickets.ParsingTicket", "Ticket")
                        .WithOne("Meta")
                        .HasForeignKey("Nude.Models.Tickets.ParsingMeta", "TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("Nude.Models.Tickets.ParsingResult", b =>
                {
                    b.HasOne("Nude.Models.Tickets.ParsingTicket", "Ticket")
                        .WithOne("Result")
                        .HasForeignKey("Nude.Models.Tickets.ParsingResult", "TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("Nude.Models.Tickets.Subscriber", b =>
                {
                    b.HasOne("Nude.Models.Tickets.FeedBackInfo", "FeedBackInfo")
                        .WithMany()
                        .HasForeignKey("FeedBackInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FeedBackInfo");
                });

            modelBuilder.Entity("ParsingTicketSubscriber", b =>
                {
                    b.HasOne("Nude.Models.Tickets.Subscriber", null)
                        .WithMany()
                        .HasForeignKey("SubscribersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nude.Models.Tickets.ParsingTicket", null)
                        .WithMany()
                        .HasForeignKey("TicketsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Nude.Models.Authors.Author", b =>
                {
                    b.Navigation("Mangas");
                });

            modelBuilder.Entity("Nude.Models.Mangas.Manga", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("Nude.Models.Sources.Source", b =>
                {
                    b.Navigation("Mangas");
                });

            modelBuilder.Entity("Nude.Models.Tickets.ParsingTicket", b =>
                {
                    b.Navigation("Meta")
                        .IsRequired();

                    b.Navigation("Result")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
