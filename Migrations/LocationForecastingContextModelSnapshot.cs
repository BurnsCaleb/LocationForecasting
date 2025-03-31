﻿// <auto-generated />
using System;
using LocationForecasting.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LocationForecasting.Migrations
{
    [DbContext(typeof(LocationForecastingContext))]
    partial class LocationForecastingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LocationForecasting.Models.ExceptionLog", b =>
                {
                    b.Property<int>("SettingsId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SettingsId"));

                    b.Property<string>("ExceptionMessage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExceptionTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("StackTrace")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SettingsId");

                    b.ToTable("ExceptionLogs");
                });

            modelBuilder.Entity("LocationForecasting.Models.KeyStorage", b =>
                {
                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Key");

                    b.ToTable("KeyStorage");
                });

            modelBuilder.Entity("LocationForecasting.Models.RequestLog", b =>
                {
                    b.Property<int>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RequestId"));

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<DateTime>("RequestTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("RequestUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResponseData")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ResponseStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RequestId");

                    b.ToTable("RequestLogs");
                });

            modelBuilder.Entity("LocationForecasting.Models.User", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
