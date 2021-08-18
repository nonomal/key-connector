﻿// <auto-generated />
using System;
using Bit.CryptoAgent.Repositories.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bit.CryptoAgent.Migrations.SqlServer
{
    [DbContext(typeof(SqlServerDatabaseContext))]
    [Migration("20210817205227_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.9")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Bit.CryptoAgent.Repositories.EntityFramework.ApplicationData", b =>
                {
                    b.Property<string>("SymmetricKey")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("ApplicationDatas");
                });

            modelBuilder.Entity("Bit.CryptoAgent.Repositories.EntityFramework.UserKey", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastAccessDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("RevisionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("UserKeys");
                });
#pragma warning restore 612, 618
        }
    }
}