﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using skcyDMSCataloguing.DAL;

namespace skcyDMSCataloguing.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20201009123030_initialmodel")]
    partial class initialmodel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("skcyDMSCataloguing.Models.Box", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoxCreatedBy")
                        .HasColumnType("int");

                    b.Property<int>("BoxCreatorID")
                        .HasColumnType("int");

                    b.Property<string>("BoxDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateBoxCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("ID");

                    b.HasIndex("BoxCreatorID");

                    b.ToTable("Box");
                });

            modelBuilder.Entity("skcyDMSCataloguing.Models.BoxCreator", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CreatorSynthesisID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.ToTable("BoxCreator");
                });

            modelBuilder.Entity("skcyDMSCataloguing.Models.CustAccData", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CustAccountNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasAlternateKey("CustAccountNo");

                    b.ToTable("BankAccount");
                });

            modelBuilder.Entity("skcyDMSCataloguing.Models.CustData", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CIFNo")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CustomerIDN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerNo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("skcyDMSCataloguing.Models.CustRelData", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CustAccDataID")
                        .HasColumnType("int");

                    b.Property<string>("CustAccountRelationCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustAccountStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustAccountType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CustDataID")
                        .HasColumnType("int");

                    b.Property<string>("CustOldAccountNo")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CustAccDataID");

                    b.HasIndex("CustDataID");

                    b.ToTable("CustomerAccount");
                });

            modelBuilder.Entity("skcyDMSCataloguing.Models.Folder", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoxID")
                        .HasColumnType("int");

                    b.Property<int>("CustDataID")
                        .HasColumnType("int");

                    b.Property<string>("FolderDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FolderName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("BoxID");

                    b.HasIndex("CustDataID");

                    b.ToTable("Folder");
                });

            modelBuilder.Entity("skcyDMSCataloguing.Models.PrjHelix1", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CustDataCIFNo")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Helix1Pool")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("CustDataCIFNo");

                    b.ToTable("PrjHelix1");
                });

            modelBuilder.Entity("skcyDMSCataloguing.Models.PrjVelocity1", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CustDataCIFNo")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("CustDataCIFNo");

                    b.ToTable("PrjVelocity1");
                });

            modelBuilder.Entity("skcyDMSCataloguing.Models.PrjVelocity2", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CustDataCIFNo")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ID");

                    b.HasIndex("CustDataCIFNo");

                    b.ToTable("PrjVelocity2");
                });

            modelBuilder.Entity("skcyDMSCataloguing.Models.Box", b =>
                {
                    b.HasOne("skcyDMSCataloguing.Models.BoxCreator", "BoxCreator")
                        .WithMany("Boxes")
                        .HasForeignKey("BoxCreatorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("skcyDMSCataloguing.Models.CustRelData", b =>
                {
                    b.HasOne("skcyDMSCataloguing.Models.CustAccData", "CustAccData")
                        .WithMany("CustRelDataEntries")
                        .HasForeignKey("CustAccDataID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("skcyDMSCataloguing.Models.CustData", "CustData")
                        .WithMany("CustRelDataEntries")
                        .HasForeignKey("CustDataID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("skcyDMSCataloguing.Models.Folder", b =>
                {
                    b.HasOne("skcyDMSCataloguing.Models.Box", "Box")
                        .WithMany("Folders")
                        .HasForeignKey("BoxID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("skcyDMSCataloguing.Models.CustData", "CustData")
                        .WithMany("Folders")
                        .HasForeignKey("CustDataID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("skcyDMSCataloguing.Models.PrjHelix1", b =>
                {
                    b.HasOne("skcyDMSCataloguing.Models.CustData", "CustData")
                        .WithMany("PrjHelixes1")
                        .HasForeignKey("CustDataCIFNo")
                        .HasPrincipalKey("CIFNo");
                });

            modelBuilder.Entity("skcyDMSCataloguing.Models.PrjVelocity1", b =>
                {
                    b.HasOne("skcyDMSCataloguing.Models.CustData", "CustData")
                        .WithMany("PrjVelocities1")
                        .HasForeignKey("CustDataCIFNo")
                        .HasPrincipalKey("CIFNo");
                });

            modelBuilder.Entity("skcyDMSCataloguing.Models.PrjVelocity2", b =>
                {
                    b.HasOne("skcyDMSCataloguing.Models.CustData", "CustData")
                        .WithMany("PrjVelocities2")
                        .HasForeignKey("CustDataCIFNo")
                        .HasPrincipalKey("CIFNo");
                });
#pragma warning restore 612, 618
        }
    }
}