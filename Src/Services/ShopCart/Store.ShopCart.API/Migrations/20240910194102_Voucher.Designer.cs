﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Store.ShopCart.API.Domain.Data.Contexts;

#nullable disable

namespace Store.ShopCart.API.Migrations
{
    [DbContext(typeof(CartDbContext))]
    [Migration("20240910194102_Voucher")]
    partial class Voucher
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Store.ShopCart.API.Domain.Data.Entitys.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("HasVoucher")
                        .HasColumnType("bit");

                    b.Property<Guid>("IdCustomer")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("IdCustomer")
                        .HasDatabaseName("IDX_Customer");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("Store.ShopCart.API.Domain.Data.Entitys.CartItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdCart")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("IdProduct")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("IdCart");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("Store.ShopCart.API.Domain.Data.Entitys.Cart", b =>
                {
                    b.OwnsOne("Store.ShopCart.API.Domain.Data.Entitys.Voucher", "Voucher", b1 =>
                        {
                            b1.Property<Guid>("CartId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Code")
                                .HasColumnType("varchar(50)")
                                .HasColumnName("Voucher_Code");

                            b1.Property<decimal?>("Discount")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<int?>("DiscountType")
                                .HasColumnType("int");

                            b1.Property<decimal?>("Percentage")
                                .HasColumnType("decimal(18,2)");

                            b1.HasKey("CartId");

                            b1.ToTable("Carts");

                            b1.WithOwner()
                                .HasForeignKey("CartId");
                        });

                    b.Navigation("Voucher")
                        .IsRequired();
                });

            modelBuilder.Entity("Store.ShopCart.API.Domain.Data.Entitys.CartItem", b =>
                {
                    b.HasOne("Store.ShopCart.API.Domain.Data.Entitys.Cart", "Cart")
                        .WithMany("Items")
                        .HasForeignKey("IdCart")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");
                });

            modelBuilder.Entity("Store.ShopCart.API.Domain.Data.Entitys.Cart", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
