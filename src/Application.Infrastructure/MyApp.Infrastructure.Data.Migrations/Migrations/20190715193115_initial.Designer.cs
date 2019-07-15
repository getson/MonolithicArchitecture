﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyApp.Infrastructure.Data;

namespace MyApp.Infrastructure.Data.Migrations.Migrations
{
    [DbContext(typeof(MyAppObjectContext))]
    [Migration("20190715193115_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyApp.Domain.Example.BankAccountAgg.BankAccount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Balance");

                    b.Property<Guid>("CustomerId");

                    b.Property<bool>("Locked");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("BankAccount");
                });

            modelBuilder.Entity("MyApp.Domain.Example.BankAccountAgg.BankAccountActivity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ActivityDescription");

                    b.Property<decimal>("Amount");

                    b.Property<Guid>("BankAccountId");

                    b.Property<DateTime>("Date");

                    b.HasKey("Id");

                    b.HasIndex("BankAccountId");

                    b.ToTable("BankAccountActivity");
                });

            modelBuilder.Entity("MyApp.Domain.Example.CountryAgg.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CountryIsoCode");

                    b.Property<string>("CountryName");

                    b.HasKey("Id");

                    b.ToTable("Country");
                });

            modelBuilder.Entity("MyApp.Domain.Example.CustomerAgg.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Company");

                    b.Property<Guid>("CountryId");

                    b.Property<decimal>("CreditLimit");

                    b.Property<string>("FirstName");

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("LastName");

                    b.Property<Guid?>("PictureId");

                    b.Property<string>("Telephone");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.HasIndex("PictureId");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("MyApp.Domain.Example.CustomerAgg.Picture", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("RawPhoto");

                    b.HasKey("Id");

                    b.ToTable("Picture");
                });

            modelBuilder.Entity("MyApp.Domain.Example.OrderAgg.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("CustomerId");

                    b.Property<DateTime?>("DeliveryDate");

                    b.Property<bool>("IsDelivered");

                    b.Property<DateTime>("OrderDate");

                    b.Property<int>("SequenceNumberOrder");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("MyApp.Domain.Example.OrderAgg.OrderLine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<decimal>("Discount");

                    b.Property<Guid>("OrderId");

                    b.Property<Guid>("ProductId");

                    b.Property<decimal>("UnitPrice");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderLine");
                });

            modelBuilder.Entity("MyApp.Domain.Example.ProductAgg.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AmountInStock");

                    b.Property<string>("Description");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Title");

                    b.Property<decimal>("UnitPrice");

                    b.HasKey("Id");

                    b.ToTable("Product");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Product");
                });

            modelBuilder.Entity("MyApp.Domain.Example.ProductAgg.Book", b =>
                {
                    b.HasBaseType("MyApp.Domain.Example.ProductAgg.Product");

                    b.Property<string>("Isbn");

                    b.Property<string>("Publisher");

                    b.ToTable("Book");

                    b.HasDiscriminator().HasValue("Book");
                });

            modelBuilder.Entity("MyApp.Domain.Example.ProductAgg.Software", b =>
                {
                    b.HasBaseType("MyApp.Domain.Example.ProductAgg.Product");

                    b.Property<string>("LicenseCode");

                    b.ToTable("Software");

                    b.HasDiscriminator().HasValue("Software");
                });

            modelBuilder.Entity("MyApp.Domain.Example.BankAccountAgg.BankAccount", b =>
                {
                    b.HasOne("MyApp.Domain.Example.CustomerAgg.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("MyApp.Domain.Example.BankAccountAgg.BankAccountNumber", "BankAccountNumber", b1 =>
                        {
                            b1.Property<Guid>("BankAccountId");

                            b1.Property<string>("AccountNumber");

                            b1.Property<string>("CheckDigits");

                            b1.Property<string>("NationalBankCode");

                            b1.Property<string>("OfficeNumber");

                            b1.HasKey("BankAccountId");

                            b1.ToTable("BankAccount");

                            b1.HasOne("MyApp.Domain.Example.BankAccountAgg.BankAccount")
                                .WithOne("BankAccountNumber")
                                .HasForeignKey("MyApp.Domain.Example.BankAccountAgg.BankAccountNumber", "BankAccountId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("MyApp.Domain.Example.BankAccountAgg.BankAccountActivity", b =>
                {
                    b.HasOne("MyApp.Domain.Example.BankAccountAgg.BankAccount")
                        .WithMany("BankAccountActivity")
                        .HasForeignKey("BankAccountId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MyApp.Domain.Example.CustomerAgg.Customer", b =>
                {
                    b.HasOne("MyApp.Domain.Example.CountryAgg.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MyApp.Domain.Example.CustomerAgg.Picture", "Picture")
                        .WithMany()
                        .HasForeignKey("PictureId");

                    b.OwnsOne("MyApp.Domain.Example.CustomerAgg.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("CustomerId");

                            b1.Property<string>("AddressLine1");

                            b1.Property<string>("AddressLine2");

                            b1.Property<string>("City");

                            b1.Property<string>("ZipCode");

                            b1.HasKey("CustomerId");

                            b1.ToTable("Customer");

                            b1.HasOne("MyApp.Domain.Example.CustomerAgg.Customer")
                                .WithOne("Address")
                                .HasForeignKey("MyApp.Domain.Example.CustomerAgg.Address", "CustomerId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("MyApp.Domain.Example.OrderAgg.Order", b =>
                {
                    b.HasOne("MyApp.Domain.Example.CustomerAgg.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.OwnsOne("MyApp.Domain.Example.OrderAgg.ShippingInfo", "ShippingInformation", b1 =>
                        {
                            b1.Property<Guid>("OrderId");

                            b1.Property<string>("ShippingAddress");

                            b1.Property<string>("ShippingCity");

                            b1.Property<string>("ShippingName");

                            b1.Property<string>("ShippingZipCode");

                            b1.HasKey("OrderId");

                            b1.ToTable("Order");

                            b1.HasOne("MyApp.Domain.Example.OrderAgg.Order")
                                .WithOne("ShippingInformation")
                                .HasForeignKey("MyApp.Domain.Example.OrderAgg.ShippingInfo", "OrderId")
                                .OnDelete(DeleteBehavior.Cascade);
                        });
                });

            modelBuilder.Entity("MyApp.Domain.Example.OrderAgg.OrderLine", b =>
                {
                    b.HasOne("MyApp.Domain.Example.OrderAgg.Order")
                        .WithMany("OrderLines")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MyApp.Domain.Example.ProductAgg.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
