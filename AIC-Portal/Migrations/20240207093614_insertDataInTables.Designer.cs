﻿// <auto-generated />
using AIC.Portal.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AICPortal.Migrations
{
    [DbContext(typeof(ContactFormContext))]
    [Migration("20240207093614_insertDataInTables")]
    partial class insertDataInTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AIC.Portal.Entities.ContactForm", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("EmailAddress");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)")
                        .HasColumnName("FullName");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("message");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(11)
                        .HasColumnType("nvarchar(11)")
                        .HasColumnName("PhoneNumber");

                    b.Property<int>("governoratesId")
                        .HasColumnType("int");

                    b.Property<int>("typeId")
                        .HasColumnType("int");

                    b.Property<string>("work")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("work");

                    b.HasKey("Id");

                    b.HasIndex("governoratesId");

                    b.HasIndex("typeId");

                    b.ToTable("ContactForm", (string)null);
                });

            modelBuilder.Entity("AIC.Portal.Entities.Countries", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("CountryName");

                    b.Property<string>("CountryNameAR")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Countries", (string)null);
                });

            modelBuilder.Entity("AIC.Portal.Entities.governorates", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CountriesId")
                        .HasColumnType("int");

                    b.Property<string>("GovernorateNameAr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("GovernorateNameAr");

                    b.Property<string>("GovernorateNameEn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("GovernorateNameEn");

                    b.Property<byte>("province_id")
                        .HasColumnType("tinyint")
                        .HasColumnName("province_id");

                    b.HasKey("Id");

                    b.HasIndex("CountriesId");

                    b.ToTable("Governorates", (string)null);
                });

            modelBuilder.Entity("AIC.Portal.Entities.type", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("Type");

                    b.Property<string>("TypeAR")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Types", (string)null);
                });

            modelBuilder.Entity("AIC.Portal.Entities.ContactForm", b =>
                {
                    b.HasOne("AIC.Portal.Entities.governorates", "city")
                        .WithMany("ContactForms")
                        .HasForeignKey("governoratesId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("AIC.Portal.Entities.type", "Type")
                        .WithMany("ContactForms")
                        .HasForeignKey("typeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Type");

                    b.Navigation("city");
                });

            modelBuilder.Entity("AIC.Portal.Entities.governorates", b =>
                {
                    b.HasOne("AIC.Portal.Entities.Countries", "country")
                        .WithMany("Governorates")
                        .HasForeignKey("CountriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("country");
                });

            modelBuilder.Entity("AIC.Portal.Entities.Countries", b =>
                {
                    b.Navigation("Governorates");
                });

            modelBuilder.Entity("AIC.Portal.Entities.governorates", b =>
                {
                    b.Navigation("ContactForms");
                });

            modelBuilder.Entity("AIC.Portal.Entities.type", b =>
                {
                    b.Navigation("ContactForms");
                });
#pragma warning restore 612, 618
        }
    }
}
