﻿// <auto-generated />
using System;
using MBBSInternship.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MBBSInternship.Migrations
{
    [DbContext(typeof(InternshipContext))]
    [Migration("20190806060316_PersonHospitalRelationshipMapping")]
    partial class PersonHospitalRelationshipMapping
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MBBSInternship.Models.District", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Districts");
                });

            modelBuilder.Entity("MBBSInternship.Models.Hospital", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DistrictId");

                    b.Property<string>("Name");

                    b.Property<int?>("TotalSlots");

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.ToTable("Hospitals");
                });

            modelBuilder.Entity("MBBSInternship.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Gender");

                    b.Property<string>("NIC");

                    b.Property<string>("Name");

                    b.Property<int>("Rank");

                    b.Property<int>("UniversityId");

                    b.HasKey("Id");

                    b.HasIndex("UniversityId");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("MBBSInternship.Models.PersonHospitalRelationship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("HospitalId");

                    b.Property<int>("PersonId");

                    b.Property<int>("PreferenceNumber");

                    b.HasKey("Id");

                    b.HasIndex("HospitalId");

                    b.HasIndex("PersonId");

                    b.ToTable("PersonHospitalRelationships");
                });

            modelBuilder.Entity("MBBSInternship.Models.University", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DistrictId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("DistrictId");

                    b.ToTable("Universities");
                });

            modelBuilder.Entity("MBBSInternship.Models.Hospital", b =>
                {
                    b.HasOne("MBBSInternship.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MBBSInternship.Models.Person", b =>
                {
                    b.HasOne("MBBSInternship.Models.University", "University")
                        .WithMany()
                        .HasForeignKey("UniversityId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MBBSInternship.Models.PersonHospitalRelationship", b =>
                {
                    b.HasOne("MBBSInternship.Models.Hospital", "Hospital")
                        .WithMany()
                        .HasForeignKey("HospitalId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MBBSInternship.Models.Person", "Person")
                        .WithMany("HospitalChoices")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MBBSInternship.Models.University", b =>
                {
                    b.HasOne("MBBSInternship.Models.District", "District")
                        .WithMany()
                        .HasForeignKey("DistrictId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}