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
    [Migration("20230225233656_DefaultedIsAdminToFalse")]
    partial class DefaultedIsAdminToFalse
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

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

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.Shoe", b =>
                {
                    b.Property<int>("ShoeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DatePosted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsSold")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("tinyint(1)")
                        .HasDefaultValue(false);

                    b.Property<double?>("LeftSize")
                        .HasColumnType("double");

                    b.Property<int>("ModelId")
                        .HasColumnType("int");

                    b.Property<double?>("RightSize")
                        .HasColumnType("double");

                    b.Property<int?>("SoldToId")
                        .HasColumnType("int");

                    b.Property<int?>("SoldToUserUserId")
                        .HasColumnType("int");

                    b.HasKey("ShoeId");

                    b.HasIndex("ModelId");

                    b.HasIndex("SoldToUserUserId");

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

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.Shoe", b =>
                {
                    b.HasOne("ShoelessJoeAPI.DataAccess.DataModels.Model", "Model")
                        .WithMany("Shoes")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ShoelessJoeAPI.DataAccess.DataModels.User", "SoldToUser")
                        .WithMany("SoldToShoes")
                        .HasForeignKey("SoldToUserUserId");

                    b.Navigation("Model");

                    b.Navigation("SoldToUser");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.Manufacter", b =>
                {
                    b.Navigation("Models");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.Model", b =>
                {
                    b.Navigation("Shoes");
                });

            modelBuilder.Entity("ShoelessJoeAPI.DataAccess.DataModels.User", b =>
                {
                    b.Navigation("Manufacters");

                    b.Navigation("SoldToShoes");
                });
#pragma warning restore 612, 618
        }
    }
}