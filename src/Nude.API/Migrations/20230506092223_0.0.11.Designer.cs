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
    [Migration("20230506092223_0.0.11")]
    partial class _0011
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ImageEntryTag", b =>
                {
                    b.Property<int>("ImagesId")
                        .HasColumnType("integer")
                        .HasColumnName("images_id");

                    b.Property<int>("TagsId")
                        .HasColumnType("integer")
                        .HasColumnName("tags_id");

                    b.HasKey("ImagesId", "TagsId")
                        .HasName("pk_image_entry_tag");

                    b.HasIndex("TagsId")
                        .HasDatabaseName("ix_image_entry_tag_tags_id");

                    b.ToTable("image_entry_tag", (string)null);
                });

            modelBuilder.Entity("MangaEntryTag", b =>
                {
                    b.Property<int>("MangasId")
                        .HasColumnType("integer")
                        .HasColumnName("mangas_id");

                    b.Property<int>("TagsId")
                        .HasColumnType("integer")
                        .HasColumnName("tags_id");

                    b.HasKey("MangasId", "TagsId")
                        .HasName("pk_manga_entry_tag");

                    b.HasIndex("TagsId")
                        .HasDatabaseName("ix_manga_entry_tag_tags_id");

                    b.ToTable("manga_entry_tag", (string)null);
                });

            modelBuilder.Entity("Nude.API.Models.Claims.ClaimEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Issuer")
                        .HasColumnType("text")
                        .HasColumnName("issuer");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_claims");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_claims_user_id");

                    b.ToTable("claims", (string)null);
                });

            modelBuilder.Entity("Nude.API.Models.Collections.CollectionImageEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CollectionId")
                        .HasColumnType("integer")
                        .HasColumnName("collection_id");

                    b.Property<int>("EntryId")
                        .HasColumnType("integer")
                        .HasColumnName("entry_id");

                    b.HasKey("Id")
                        .HasName("pk_collection_image_entry");

                    b.HasIndex("CollectionId")
                        .HasDatabaseName("ix_collection_image_entry_collection_id");

                    b.HasIndex("EntryId")
                        .HasDatabaseName("ix_collection_image_entry_entry_id");

                    b.ToTable("collection_image_entry", (string)null);
                });

            modelBuilder.Entity("Nude.API.Models.Collections.ImageCollection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentKey")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content_key");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_image_collections");

                    b.ToTable("image_collections", (string)null);
                });

            modelBuilder.Entity("Nude.API.Models.Formats.Format", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("discriminator");

                    b.Property<int?>("ImageCollectionId")
                        .HasColumnType("integer")
                        .HasColumnName("image_collection_id");

                    b.Property<int?>("MangaEntryId")
                        .HasColumnType("integer")
                        .HasColumnName("manga_entry_id");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.HasKey("Id")
                        .HasName("pk_formats");

                    b.HasIndex("ImageCollectionId")
                        .HasDatabaseName("ix_formats_image_collection_id");

                    b.HasIndex("MangaEntryId")
                        .HasDatabaseName("ix_formats_manga_entry_id");

                    b.ToTable("formats", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("Format");
                });

            modelBuilder.Entity("Nude.API.Models.Images.ImageEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentKey")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content_key");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<int?>("ExternalMetaId")
                        .HasColumnType("integer")
                        .HasColumnName("external_meta_id");

                    b.Property<int?>("OwnerId")
                        .HasColumnType("integer")
                        .HasColumnName("owner_id");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("url");

                    b.HasKey("Id")
                        .HasName("pk_images");

                    b.HasIndex("ExternalMetaId")
                        .HasDatabaseName("ix_images_external_meta_id");

                    b.HasIndex("OwnerId")
                        .HasDatabaseName("ix_images_owner_id");

                    b.ToTable("images", (string)null);
                });

            modelBuilder.Entity("Nude.API.Models.Mangas.MangaEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentKey")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content_key");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int?>("ExternalMetaId")
                        .HasColumnType("integer")
                        .HasColumnName("external_meta_id");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("pk_mangas");

                    b.HasIndex("ExternalMetaId")
                        .HasDatabaseName("ix_mangas_external_meta_id");

                    b.ToTable("mangas", (string)null);
                });

            modelBuilder.Entity("Nude.API.Models.Mangas.MangaImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("MangaEntryId")
                        .HasColumnType("integer")
                        .HasColumnName("manga_entry_id");

                    b.Property<int>("UrlId")
                        .HasColumnType("integer")
                        .HasColumnName("url_id");

                    b.HasKey("Id")
                        .HasName("pk_manga_images");

                    b.HasIndex("MangaEntryId")
                        .HasDatabaseName("ix_manga_images_manga_entry_id");

                    b.HasIndex("UrlId")
                        .HasDatabaseName("ix_manga_images_url_id");

                    b.ToTable("manga_images", (string)null);
                });

            modelBuilder.Entity("Nude.API.Models.Mangas.Meta.ExternalMeta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("SourceId")
                        .HasColumnType("text")
                        .HasColumnName("source_id");

                    b.Property<string>("SourceUrl")
                        .HasColumnType("text")
                        .HasColumnName("source_url");

                    b.HasKey("Id")
                        .HasName("pk_external_metas");

                    b.ToTable("external_metas", (string)null);
                });

            modelBuilder.Entity("Nude.API.Models.Tags.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("NormalizeValue")
                        .HasColumnType("text")
                        .HasColumnName("normalize_value");

                    b.Property<int>("Type")
                        .HasColumnType("integer")
                        .HasColumnName("type");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_tags");

                    b.ToTable("tags", (string)null);
                });

            modelBuilder.Entity("Nude.API.Models.Tickets.ContentTicket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ContentKey")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content_key");

                    b.Property<string>("ContentUrl")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content_url");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_content_tickets");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_content_tickets_user_id");

                    b.ToTable("content_tickets", (string)null);
                });

            modelBuilder.Entity("Nude.API.Models.Urls.Url", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_urls");

                    b.ToTable("urls", (string)null);
                });

            modelBuilder.Entity("Nude.API.Models.Users.Accounts.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("discriminator");

                    b.Property<int>("OwnerId")
                        .HasColumnType("integer")
                        .HasColumnName("owner_id");

                    b.HasKey("Id")
                        .HasName("pk_accounts");

                    b.HasIndex("OwnerId")
                        .HasDatabaseName("ix_accounts_owner_id");

                    b.ToTable("accounts", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("Account");
                });

            modelBuilder.Entity("Nude.API.Models.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("Nude.API.Models.Views.View", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("ImageCollectionId")
                        .HasColumnType("integer")
                        .HasColumnName("image_collection_id");

                    b.Property<int?>("ImageId")
                        .HasColumnType("integer")
                        .HasColumnName("image_id");

                    b.Property<int?>("MangaId")
                        .HasColumnType("integer")
                        .HasColumnName("manga_id");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_view");

                    b.HasIndex("ImageCollectionId")
                        .HasDatabaseName("ix_view_image_collection_id");

                    b.HasIndex("ImageId")
                        .HasDatabaseName("ix_view_image_id");

                    b.HasIndex("MangaId")
                        .HasDatabaseName("ix_view_manga_id");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_view_user_id");

                    b.ToTable("view", (string)null);
                });

            modelBuilder.Entity("Nude.API.Models.Formats.TelegraphFormat", b =>
                {
                    b.HasBaseType("Nude.API.Models.Formats.Format");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("url");

                    b.HasDiscriminator().HasValue("TelegraphFormat");
                });

            modelBuilder.Entity("Nude.API.Models.Users.Accounts.TelegramAccount", b =>
                {
                    b.HasBaseType("Nude.API.Models.Users.Accounts.Account");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasDiscriminator().HasValue("TelegramAccount");
                });

            modelBuilder.Entity("ImageEntryTag", b =>
                {
                    b.HasOne("Nude.API.Models.Images.ImageEntry", null)
                        .WithMany()
                        .HasForeignKey("ImagesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_image_entry_tag_images_images_id");

                    b.HasOne("Nude.API.Models.Tags.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_image_entry_tag_tags_tags_id");
                });

            modelBuilder.Entity("MangaEntryTag", b =>
                {
                    b.HasOne("Nude.API.Models.Mangas.MangaEntry", null)
                        .WithMany()
                        .HasForeignKey("MangasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_manga_entry_tag_mangas_mangas_id");

                    b.HasOne("Nude.API.Models.Tags.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_manga_entry_tag_tags_tags_id");
                });

            modelBuilder.Entity("Nude.API.Models.Claims.ClaimEntry", b =>
                {
                    b.HasOne("Nude.API.Models.Users.User", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_claims_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Nude.API.Models.Collections.CollectionImageEntry", b =>
                {
                    b.HasOne("Nude.API.Models.Collections.ImageCollection", "Collection")
                        .WithMany("Images")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_collection_image_entry_image_collections_collection_id");

                    b.HasOne("Nude.API.Models.Images.ImageEntry", "Entry")
                        .WithMany()
                        .HasForeignKey("EntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_collection_image_entry_images_entry_id");

                    b.Navigation("Collection");

                    b.Navigation("Entry");
                });

            modelBuilder.Entity("Nude.API.Models.Formats.Format", b =>
                {
                    b.HasOne("Nude.API.Models.Collections.ImageCollection", "ImageCollection")
                        .WithMany("Formats")
                        .HasForeignKey("ImageCollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_formats_image_collections_image_collection_id");

                    b.HasOne("Nude.API.Models.Mangas.MangaEntry", "MangaEntry")
                        .WithMany("Formats")
                        .HasForeignKey("MangaEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("fk_formats_mangas_manga_entry_id");

                    b.Navigation("ImageCollection");

                    b.Navigation("MangaEntry");
                });

            modelBuilder.Entity("Nude.API.Models.Images.ImageEntry", b =>
                {
                    b.HasOne("Nude.API.Models.Mangas.Meta.ExternalMeta", "ExternalMeta")
                        .WithMany()
                        .HasForeignKey("ExternalMetaId")
                        .HasConstraintName("fk_images_external_metas_external_meta_id");

                    b.HasOne("Nude.API.Models.Users.User", "Owner")
                        .WithMany("Images")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_images_users_owner_id");

                    b.Navigation("ExternalMeta");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Nude.API.Models.Mangas.MangaEntry", b =>
                {
                    b.HasOne("Nude.API.Models.Mangas.Meta.ExternalMeta", "ExternalMeta")
                        .WithMany()
                        .HasForeignKey("ExternalMetaId")
                        .HasConstraintName("fk_mangas_external_metas_external_meta_id");

                    b.Navigation("ExternalMeta");
                });

            modelBuilder.Entity("Nude.API.Models.Mangas.MangaImage", b =>
                {
                    b.HasOne("Nude.API.Models.Mangas.MangaEntry", "MangaEntry")
                        .WithMany("Images")
                        .HasForeignKey("MangaEntryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_manga_images_mangas_manga_entry_id");

                    b.HasOne("Nude.API.Models.Urls.Url", "Url")
                        .WithMany()
                        .HasForeignKey("UrlId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_manga_images_urls_url_id");

                    b.Navigation("MangaEntry");

                    b.Navigation("Url");
                });

            modelBuilder.Entity("Nude.API.Models.Tickets.ContentTicket", b =>
                {
                    b.HasOne("Nude.API.Models.Users.User", "User")
                        .WithMany("ContentTickets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_content_tickets_users_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Nude.API.Models.Users.Accounts.Account", b =>
                {
                    b.HasOne("Nude.API.Models.Users.User", "Owner")
                        .WithMany("Accounts")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_accounts_users_owner_id");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Nude.API.Models.Views.View", b =>
                {
                    b.HasOne("Nude.API.Models.Collections.ImageCollection", "ImageCollection")
                        .WithMany("Views")
                        .HasForeignKey("ImageCollectionId")
                        .HasConstraintName("fk_view_image_collections_image_collection_id");

                    b.HasOne("Nude.API.Models.Images.ImageEntry", "Image")
                        .WithMany("Views")
                        .HasForeignKey("ImageId")
                        .HasConstraintName("fk_view_images_image_id");

                    b.HasOne("Nude.API.Models.Mangas.MangaEntry", "Manga")
                        .WithMany("Views")
                        .HasForeignKey("MangaId")
                        .HasConstraintName("fk_view_mangas_manga_id");

                    b.HasOne("Nude.API.Models.Users.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_view_users_user_id");

                    b.Navigation("Image");

                    b.Navigation("ImageCollection");

                    b.Navigation("Manga");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Nude.API.Models.Collections.ImageCollection", b =>
                {
                    b.Navigation("Formats");

                    b.Navigation("Images");

                    b.Navigation("Views");
                });

            modelBuilder.Entity("Nude.API.Models.Images.ImageEntry", b =>
                {
                    b.Navigation("Views");
                });

            modelBuilder.Entity("Nude.API.Models.Mangas.MangaEntry", b =>
                {
                    b.Navigation("Formats");

                    b.Navigation("Images");

                    b.Navigation("Views");
                });

            modelBuilder.Entity("Nude.API.Models.Users.User", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("Claims");

                    b.Navigation("ContentTickets");

                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
