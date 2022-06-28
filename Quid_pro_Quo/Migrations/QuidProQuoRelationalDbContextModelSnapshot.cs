﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Quid_pro_Quo.Database.Ralational;

namespace Quid_pro_Quo.Migrations
{
    [DbContext(typeof(QuidProQuoRelationalDbContext))]
    partial class QuidProQuoRelationalDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("Quid_pro_Quo.Database.Ralational.ComplaintEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ComplaintType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ComplaintedContentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Complaints");
                });

            modelBuilder.Entity("Quid_pro_Quo.Database.Ralational.LoginInfoEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("HashPassword")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("LoginInfos");
                });

            modelBuilder.Entity("Quid_pro_Quo.Database.Ralational.PostEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageFileNames")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActual")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PerformServiceInPlace")
                        .HasColumnType("TEXT");

                    b.Property<string>("PerformServiceOnDatesList")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PostedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Quid_pro_Quo.Database.Ralational.ProposalEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("RequestedPostId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RequestingPostId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RequestedPostId");

                    b.HasIndex("RequestingPostId");

                    b.ToTable("Proposals");
                });

            modelBuilder.Entity("Quid_pro_Quo.Database.Ralational.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("AvatarFileName")
                        .HasColumnType("TEXT");

                    b.Property<string>("Biographi")
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Quid_pro_Quo.Database.Ralational.ComplaintEntity", b =>
                {
                    b.HasOne("Quid_pro_Quo.Database.Ralational.UserEntity", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Quid_pro_Quo.Database.Ralational.PostEntity", b =>
                {
                    b.HasOne("Quid_pro_Quo.Database.Ralational.UserEntity", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Quid_pro_Quo.Database.Ralational.ProposalEntity", b =>
                {
                    b.HasOne("Quid_pro_Quo.Database.Ralational.UserEntity", "RequestedPost")
                        .WithMany()
                        .HasForeignKey("RequestedPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Quid_pro_Quo.Database.Ralational.UserEntity", "RequestingPost")
                        .WithMany()
                        .HasForeignKey("RequestingPostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RequestedPost");

                    b.Navigation("RequestingPost");
                });

            modelBuilder.Entity("Quid_pro_Quo.Database.Ralational.UserEntity", b =>
                {
                    b.HasOne("Quid_pro_Quo.Database.Ralational.LoginInfoEntity", "LoginInfo")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LoginInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
