﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Nude.Data.Infrastructure.Contexts;

#nullable disable

namespace Nude.API.Migrations.FixedAppDb
{
    [DbContext(typeof(FixedAppDbContext))]
    partial class FixedAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MangaEntryTag", b =>
                {
                    b.Property<int>("MangasId")
                        .HasColumnType("integer");

                    b.Property<int>("TagsId")
                        .HasColumnType("integer");

                    b.HasKey("MangasId", "TagsId");

                    b.HasIndex("TagsId");

                    b.ToTable("MangaEntryTag");
                });

            modelBuilder.Entity("Nude.API.Models.Formats.FormattedContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("MangaEntryId")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MangaEntryId");

                    b.ToTable("FormattedContents");

                    b.HasDiscriminator<string>("Discriminator").HasValue("FormattedContent");
                });

            modelBuilder.Entity("Nude.API.Models.Mangas.MangaEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ExternalMetaId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ExternalMetaId");

                    b.ToTable("Mangas");
                });

            modelBuilder.Entity("Nude.API.Models.Mangas.MangaImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MangaEntryId")
                        .HasColumnType("integer");

                    b.Property<int>("UrlId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("MangaEntryId");

                    b.HasIndex("UrlId");

                    b.ToTable("MangaImages");
                });

            modelBuilder.Entity("Nude.API.Models.Mangas.Meta.MangaExternalMeta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("SourceId")
                        .HasColumnType("text");

                    b.Property<string>("SourceUrl")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MangaExternalMetas");
                });

            modelBuilder.Entity("Nude.API.Models.Tags.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("NormalizeValue")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Nude.API.Models.Tickets.ContentFormatTicket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ContextId")
                        .HasColumnType("integer");

                    b.Property<int>("FormatType")
                        .HasColumnType("integer");

                    b.Property<int?>("ResultId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ContextId");

                    b.HasIndex("ResultId");

                    b.ToTable("FormatTickets");
                });

            modelBuilder.Entity("Nude.API.Models.Tickets.ContentTicket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ContextId")
                        .HasColumnType("integer");

                    b.Property<int?>("ResultId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ContextId");

                    b.HasIndex("ResultId");

                    b.ToTable("ContentTickets");
                });

            modelBuilder.Entity("Nude.API.Models.Tickets.Contexts.ContentFormatTicketContext", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("EntityId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EntityType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ContentFormatTicketContext");
                });

            modelBuilder.Entity("Nude.API.Models.Tickets.Contexts.ContentTicketContext", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentId")
                        .HasColumnType("text");

                    b.Property<string>("ContentUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TicketContexts");
                });

            modelBuilder.Entity("Nude.API.Models.Tickets.Results.ContentResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EntityId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("ContentResults");
                });

            modelBuilder.Entity("Nude.API.Models.Tickets.Subscribers.Subscriber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("CallbackUrl")
                        .HasColumnType("text");

                    b.Property<string>("EntityId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EntityType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Subscribers");
                });

            modelBuilder.Entity("Nude.API.Models.Urls.Url", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Urls");
                });

            modelBuilder.Entity("Nude.API.Models.Formats.TelegraphContent", b =>
                {
                    b.HasBaseType("Nude.API.Models.Formats.FormattedContent");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasDiscriminator().HasValue("TelegraphContent");
                });

            modelBuilder.Entity("MangaEntryTag", b =>
                {
                    b.HasOne("Nude.API.Models.Mangas.MangaEntry", null)
                        .WithMany()
                        .HasForeignKey("MangasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nude.API.Models.Tags.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Nude.API.Models.Formats.FormattedContent", b =>
                {
                    b.HasOne("Nude.API.Models.Mangas.MangaEntry", null)
                        .WithMany("Formats")
                        .HasForeignKey("MangaEntryId");
                });

            modelBuilder.Entity("Nude.API.Models.Mangas.MangaEntry", b =>
                {
                    b.HasOne("Nude.API.Models.Mangas.Meta.MangaExternalMeta", "ExternalMeta")
                        .WithMany()
                        .HasForeignKey("ExternalMetaId");

                    b.Navigation("ExternalMeta");
                });

            modelBuilder.Entity("Nude.API.Models.Mangas.MangaImage", b =>
                {
                    b.HasOne("Nude.API.Models.Mangas.MangaEntry", "MangaEntry")
                        .WithMany("Images")
                        .HasForeignKey("MangaEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nude.API.Models.Urls.Url", "Url")
                        .WithMany()
                        .HasForeignKey("UrlId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MangaEntry");

                    b.Navigation("Url");
                });

            modelBuilder.Entity("Nude.API.Models.Tickets.ContentFormatTicket", b =>
                {
                    b.HasOne("Nude.API.Models.Tickets.Contexts.ContentFormatTicketContext", "Context")
                        .WithMany()
                        .HasForeignKey("ContextId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nude.API.Models.Formats.FormattedContent", "Result")
                        .WithMany()
                        .HasForeignKey("ResultId");

                    b.Navigation("Context");

                    b.Navigation("Result");
                });

            modelBuilder.Entity("Nude.API.Models.Tickets.ContentTicket", b =>
                {
                    b.HasOne("Nude.API.Models.Tickets.Contexts.ContentTicketContext", "Context")
                        .WithMany()
                        .HasForeignKey("ContextId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nude.API.Models.Tickets.Results.ContentResult", "Result")
                        .WithMany()
                        .HasForeignKey("ResultId");

                    b.Navigation("Context");

                    b.Navigation("Result");
                });

            modelBuilder.Entity("Nude.API.Models.Mangas.MangaEntry", b =>
                {
                    b.Navigation("Formats");

                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
