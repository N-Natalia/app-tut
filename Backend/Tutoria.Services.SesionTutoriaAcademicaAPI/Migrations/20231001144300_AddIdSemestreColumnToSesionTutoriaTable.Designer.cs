﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Context;

#nullable disable

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231001144300_AddIdSemestreColumnToSesionTutoriaTable")]
    partial class AddIdSemestreColumnToSesionTutoriaTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Tutoria.Services.SesionTutoriaAcademicaAPI.Models.DetalleSesionTutoria", b =>
                {
                    b.Property<int>("IdDetalleSesionTutoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdDetalleSesionTutoria"));

                    b.Property<string>("Descripcion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Dimension")
                        .HasColumnType("int");

                    b.Property<int>("IdSesionTutoria")
                        .HasColumnType("int");

                    b.Property<string>("Observaciones")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Referencia")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdDetalleSesionTutoria");

                    b.HasIndex("IdSesionTutoria");

                    b.ToTable("detallesSesionTutoria", (string)null);
                });

            modelBuilder.Entity("Tutoria.Services.SesionTutoriaAcademicaAPI.Models.ProgramacionReservaObligatoria", b =>
                {
                    b.Property<int>("IdProgramacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdProgramacion"));

                    b.Property<bool>("Activo")
                        .HasColumnType("bit");

                    b.Property<int>("Duracion")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("HoraInicioSesionTutoria")
                        .HasColumnType("time");

                    b.Property<string>("IdTutor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.HasKey("IdProgramacion");

                    b.ToTable("programacionesReservaObligatoria", (string)null);
                });

            modelBuilder.Entity("Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Reserva", b =>
                {
                    b.Property<int>("IdReserva")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdReserva"));

                    b.Property<string>("EnlaceReunion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EstadoConfirmacion")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("HoraTutoria")
                        .HasColumnType("time");

                    b.Property<int>("IdCargaTutoria")
                        .HasColumnType("int");

                    b.Property<int?>("IdProgramacionReservaObligatoria")
                        .HasColumnType("int");

                    b.Property<int?>("IdTutorHorarioDisponible")
                        .HasColumnType("int");

                    b.Property<string>("LugarReunion")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TipoReserva")
                        .HasColumnType("int");

                    b.Property<int>("TipoReunion")
                        .HasColumnType("int");

                    b.HasKey("IdReserva");

                    b.HasIndex("IdProgramacionReservaObligatoria");

                    b.HasIndex("IdTutorHorarioDisponible");

                    b.ToTable("reservas", (string)null);
                });

            modelBuilder.Entity("Tutoria.Services.SesionTutoriaAcademicaAPI.Models.SesionTutoria", b =>
                {
                    b.Property<int>("IdSesionTutoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdSesionTutoria"));

                    b.Property<DateTime>("FechaReunion")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("Hora")
                        .HasColumnType("time");

                    b.Property<int>("IdReserva")
                        .HasColumnType("int");

                    b.Property<int>("IdSemestre")
                        .HasColumnType("int");

                    b.Property<string>("IdTutor")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IdTutorado")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdSesionTutoria");

                    b.HasIndex("IdReserva");

                    b.ToTable("sesionesTutoria", (string)null);
                });

            modelBuilder.Entity("Tutoria.Services.SesionTutoriaAcademicaAPI.Models.TutorHorarioDisponible", b =>
                {
                    b.Property<int>("IdHorario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdHorario"));

                    b.Property<bool>("Activo")
                        .HasColumnType("bit");

                    b.Property<string>("Dia")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Duracion")
                        .HasColumnType("int");

                    b.Property<TimeSpan>("Hora")
                        .HasColumnType("time");

                    b.Property<string>("IdTutor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.HasKey("IdHorario");

                    b.ToTable("tutorHorariosDisponible", (string)null);
                });

            modelBuilder.Entity("Tutoria.Services.SesionTutoriaAcademicaAPI.Models.DetalleSesionTutoria", b =>
                {
                    b.HasOne("Tutoria.Services.SesionTutoriaAcademicaAPI.Models.SesionTutoria", "SesionTutoria")
                        .WithMany()
                        .HasForeignKey("IdSesionTutoria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SesionTutoria");
                });

            modelBuilder.Entity("Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Reserva", b =>
                {
                    b.HasOne("Tutoria.Services.SesionTutoriaAcademicaAPI.Models.ProgramacionReservaObligatoria", "ProgramacionReservaObligatoria")
                        .WithMany()
                        .HasForeignKey("IdProgramacionReservaObligatoria");

                    b.HasOne("Tutoria.Services.SesionTutoriaAcademicaAPI.Models.TutorHorarioDisponible", "TutorHorarioDisponible")
                        .WithMany()
                        .HasForeignKey("IdTutorHorarioDisponible");

                    b.Navigation("ProgramacionReservaObligatoria");

                    b.Navigation("TutorHorarioDisponible");
                });

            modelBuilder.Entity("Tutoria.Services.SesionTutoriaAcademicaAPI.Models.SesionTutoria", b =>
                {
                    b.HasOne("Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Reserva", "Reserva")
                        .WithMany()
                        .HasForeignKey("IdReserva")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reserva");
                });
#pragma warning restore 612, 618
        }
    }
}
