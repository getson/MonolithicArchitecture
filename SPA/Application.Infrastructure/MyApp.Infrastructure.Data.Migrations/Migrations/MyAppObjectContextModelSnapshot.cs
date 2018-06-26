﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyApp.Infrastructure.Data;

namespace MyApp.Infrastructure.Data.Migrations.Migrations
{
    [DbContext(typeof(MyAppObjectContext))]
    partial class MyAppObjectContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyApp.Core.Domain.Logging.ActivityLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ActivityLogTypeId");

                    b.Property<string>("Comment")
                        .IsRequired();

                    b.Property<DateTime>("CreatedOnUtc");

                    b.Property<int?>("EntityId");

                    b.Property<string>("EntityName")
                        .HasMaxLength(400);

                    b.Property<string>("IpAddress")
                        .HasMaxLength(200);

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ActivityLogTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("ActivityLog");
                });

            modelBuilder.Entity("MyApp.Core.Domain.Logging.ActivityLogType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Enabled");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<string>("SystemKeyword")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("ActivityLogType");
                });

            modelBuilder.Entity("MyApp.Core.Domain.Logging.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedOnUtc");

                    b.Property<string>("FullMessage");

                    b.Property<string>("IpAddress")
                        .HasMaxLength(200);

                    b.Property<int>("LogLevelId");

                    b.Property<string>("PageUrl");

                    b.Property<string>("ReferrerUrl");

                    b.Property<string>("ShortMessage")
                        .IsRequired();

                    b.Property<int?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("MyApp.Core.Domain.User.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("UserName")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("MyApp.Core.Domain.Logging.ActivityLog", b =>
                {
                    b.HasOne("MyApp.Core.Domain.Logging.ActivityLogType", "ActivityLogType")
                        .WithMany()
                        .HasForeignKey("ActivityLogTypeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MyApp.Core.Domain.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MyApp.Core.Domain.Logging.Log", b =>
                {
                    b.HasOne("MyApp.Core.Domain.User.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
