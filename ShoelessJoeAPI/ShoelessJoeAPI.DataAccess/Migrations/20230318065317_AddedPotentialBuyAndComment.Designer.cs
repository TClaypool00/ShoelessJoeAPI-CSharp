﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShoelessJoeAPI.DataAccess.DataModels;

#nullable disable

namespace ShoelessJoeAPI.DataAccess.Migrations
{
    [DbContext(typeof(ShoelessJoeContext))]
    [Migration("20230318065317_AddedPotentialBuyAndComment")]
    partial class AddedPotentialBuyAndComment
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CommentText")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("DatePosted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<int>("PotentialBuyId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CommentId");

                    b.HasIndex("PotentialBuyId");

                    b.HasIndex("UserId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.Manufacter", b =>
                {
                    b.Property<int>("ManufacterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ManufacterName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ManufacterId");

                    b.HasIndex("UserId");

                    b.ToTable("Manufacters");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.Model", b =>
                {
                    b.Property<int>("ModelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ManufacterId")
                        .HasColumnType("int");

                    b.Property<string>("ModelName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("ModelId");

                    b.HasIndex("ManufacterId");

                    b.ToTable("Models");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.PotentialBuy", b =>
                {
                    b.Property<int>("PotentialBuyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateSold")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsSold")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<int>("PotentialBuyerUserId")
                        .HasColumnType("int");

                    b.Property<int>("ShoeId")
                        .HasColumnType("int");

                    b.HasKey("PotentialBuyId");

                    b.HasIndex("PotentialBuyerUserId");

                    b.HasIndex("ShoeId");

                    b.ToTable("PotentialBuy");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.Shoe", b =>
                {
                    b.Property<int>("ShoeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DatePosted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<double?>("LeftSize")
                        .HasColumnType("double");

                    b.Property<int>("ModelId")
                        .HasColumnType("int");

                    b.Property<double?>("RightSize")
                        .HasColumnType("double");

                    b.HasKey("ShoeId");

                    b.HasIndex("ModelId");

                    b.ToTable("Shoes");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<bool>("IsAdmin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PhoneNumb")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.Comment", b =>
                {
                    b.HasOne("ShoelessJoeAPI.DataAccess.DataModels.PotentialBuy", "PotentialBuy")
                        .WithMany("Comments")
                        .HasForeignKey("PotentialBuyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShoelessJoeAPI.DataAccess.DataModels.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PotentialBuy");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.Manufacter", b =>
                {
                    b.HasOne("ShoelessJoeAPI.DataAccess.DataModels.User", "User")
                        .WithMany("Manufacters")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.Model", b =>
                {
                    b.HasOne("ShoelessJoeAPI.DataAccess.DataModels.Manufacter", "Manufacter")
                        .WithMany("Models")
                        .HasForeignKey("ManufacterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Manufacter");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.PotentialBuy", b =>
                {
                    b.HasOne("ShoelessJoeAPI.DataAccess.DataModels.User", "PotentialBuyer")
                        .WithMany("PotentialBuys")
                        .HasForeignKey("PotentialBuyerUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShoelessJoeAPI.DataAccess.DataModels.Shoe", "Shoe")
                        .WithMany("PotentialBuys")
                        .HasForeignKey("ShoeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PotentialBuyer");

                    b.Navigation("Shoe");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.Shoe", b =>
                {
                    b.HasOne("ShoelessJoeAPI.DataAccess.DataModels.Model", "Model")
                        .WithMany("Shoes")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Model");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.Manufacter", b =>
                {
                    b.Navigation("Models");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.Model", b =>
                {
                    b.Navigation("Shoes");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.PotentialBuy", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.Shoe", b =>
                {
                    b.Navigation("PotentialBuys");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.User", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Manufacters");

                    b.Navigation("PotentialBuys");
                });
#pragma warning restore 612, 618
        }
    }
}
