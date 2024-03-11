﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ViajesAPIContext))]
    [Migration("20240309031631_migration10")]
    partial class migration10
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Core.Entities.Flight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ArrivalStation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DepartureStation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdTransportFk")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int?>("TransportId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TransportId");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("Core.Entities.Journey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Destination")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Origin")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Journeys");
                });

            modelBuilder.Entity("Core.Entities.Transport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FlightCarrier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FlightNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Transports");
                });

            modelBuilder.Entity("FlightJourney", b =>
                {
                    b.Property<int>("FlightsId")
                        .HasColumnType("int");

                    b.Property<int>("JourneysId")
                        .HasColumnType("int");

                    b.HasKey("FlightsId", "JourneysId");

                    b.HasIndex("JourneysId");

                    b.ToTable("FlightJourney");
                });

            modelBuilder.Entity("Core.Entities.Flight", b =>
                {
                    b.HasOne("Core.Entities.Transport", "Transport")
                        .WithMany("Flights")
                        .HasForeignKey("TransportId");

                    b.Navigation("Transport");
                });

            modelBuilder.Entity("FlightJourney", b =>
                {
                    b.HasOne("Core.Entities.Flight", null)
                        .WithMany()
                        .HasForeignKey("FlightsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.Journey", null)
                        .WithMany()
                        .HasForeignKey("JourneysId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.Transport", b =>
                {
                    b.Navigation("Flights");
                });
#pragma warning restore 612, 618
        }
    }
}
