﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PortaleCorsi.Context;

#nullable disable

namespace PortaleCorsi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PortaleCorsi.DbEntities.AnagraficaIndirizzo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Alias")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("AnagraficaId")
                        .HasColumnType("int");

                    b.Property<string>("Cap")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("Citta")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Civico")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Indirizzo")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Prov")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("AnagraficaId");

                    b.ToTable("AnagraficaIndirizzo", (string)null);
                });

            modelBuilder.Entity("PortaleCorsi.DbEntities.AnagraficaMaster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CodiceFiscale")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("Cognome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.HasIndex("CodiceFiscale")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("CodiceFiscale"), false);

                    b.ToTable("AnagraficaMaster", (string)null);
                });

            modelBuilder.Entity("PortaleCorsi.DbEntities.CorsoIscrizione", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AnagraficaId")
                        .HasColumnType("int");

                    b.Property<int>("CorsoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataIscrizione")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AnagraficaId");

                    b.HasIndex("CorsoId");

                    b.ToTable("CorsoIscrizione");
                });

            modelBuilder.Entity("PortaleCorsi.DbEntities.CorsoLezione", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CorsoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataOraFine")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("DataOraInizio")
                        .HasColumnType("datetime");

                    b.Property<string>("Descrizione")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CorsoId");

                    b.ToTable("CorsoLezione");
                });

            modelBuilder.Entity("PortaleCorsi.DbEntities.CorsoMaster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Codice")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<DateTime>("DataCreazione")
                        .HasColumnType("date");

                    b.Property<DateTime>("DataFineIscrizioni")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("DataInizio")
                        .HasColumnType("date");

                    b.Property<string>("Descrizione")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IscrizioniChiuse")
                        .HasColumnType("bit");

                    b.Property<string>("LuogoLezioni")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaxPartecipanti")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("OnLine")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("CorsoMaster");
                });

            modelBuilder.Entity("PortaleCorsi.DbEntities.AnagraficaIndirizzo", b =>
                {
                    b.HasOne("PortaleCorsi.DbEntities.AnagraficaMaster", "AnagraficaMaster")
                        .WithMany("Indirizzi")
                        .HasForeignKey("AnagraficaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnagraficaMaster");
                });

            modelBuilder.Entity("PortaleCorsi.DbEntities.CorsoIscrizione", b =>
                {
                    b.HasOne("PortaleCorsi.DbEntities.AnagraficaMaster", "AnagraficaMaster")
                        .WithMany("Iscrizioni")
                        .HasForeignKey("AnagraficaId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PortaleCorsi.DbEntities.CorsoMaster", "CorsoMaster")
                        .WithMany("Iscrizioni")
                        .HasForeignKey("CorsoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AnagraficaMaster");

                    b.Navigation("CorsoMaster");
                });

            modelBuilder.Entity("PortaleCorsi.DbEntities.CorsoLezione", b =>
                {
                    b.HasOne("PortaleCorsi.DbEntities.CorsoMaster", "CorsoMaster")
                        .WithMany("Lezioni")
                        .HasForeignKey("CorsoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("CorsoMaster");
                });

            modelBuilder.Entity("PortaleCorsi.DbEntities.AnagraficaMaster", b =>
                {
                    b.Navigation("Indirizzi");

                    b.Navigation("Iscrizioni");
                });

            modelBuilder.Entity("PortaleCorsi.DbEntities.CorsoMaster", b =>
                {
                    b.Navigation("Iscrizioni");

                    b.Navigation("Lezioni");
                });
#pragma warning restore 612, 618
        }
    }
}