using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models;

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<TutorHorarioDisponible> TutorHorariosDisponible { get; set; }
        public DbSet<ProgramacionReservaObligatoria> ProgramacionesReservaObligatoria { get; set; }
        public DbSet<DetalleProgramacionReservaObligatoria> DetalleProgramacionReservaObligatoria { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<SesionTutoria> SesionesTutoria { get; set; }
        public DbSet<DetalleSesionTutoria> DetalleSesionesTutoria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TutorHorarioDisponible>()
                        .ToTable("tutorHorariosDisponible")
                        .Property(p => p.Hora)
                        .HasConversion(new TimeOnlyConverter());

            modelBuilder.Entity<ProgramacionReservaObligatoria>()
                        .ToTable("programacionesReservaObligatoria")
                        .Property(p => p.FechaInicio)
                        .HasConversion(new DateOnlyConverter());

            modelBuilder.Entity<ProgramacionReservaObligatoria>()
                        .ToTable("programacionesReservaObligatoria")
                        .Property(p => p.FechaFin)
                        .HasConversion(new DateOnlyConverter());


            modelBuilder.Entity<DetalleProgramacionReservaObligatoria>()
                        .ToTable("detallesProgramacionReservaObligatoria")
                        .Property(p => p.Fecha)
                        .HasConversion(new DateOnlyConverter());

            modelBuilder.Entity<DetalleProgramacionReservaObligatoria>()
                        .ToTable("detallesProgramacionReservaObligatoria")
                        .Property(p => p.HoraInicioSesionTutoria)
                        .HasConversion(new TimeOnlyConverter());


            modelBuilder.Entity<Reserva>()
                        .ToTable("reservas")
                        .Property(p => p.Fecha)
                        .HasConversion(new DateOnlyConverter());

            modelBuilder.Entity<Reserva>()
                        .ToTable("reservas")
                        .Property(p => p.HoraTutoria)
                        .HasConversion(new TimeOnlyConverter());

                       
            modelBuilder.Entity<SesionTutoria>()
                        .ToTable("sesionesTutoria")
                        .Property(p => p.FechaReunion)
                        .HasConversion(new DateOnlyConverter());



            modelBuilder.Entity<SesionTutoria>()
                        .ToTable("sesionesTutoria")
                        .Property(p => p.Hora)
                        .HasConversion(new TimeOnlyConverter());


            modelBuilder.Entity<DetalleSesionTutoria>().ToTable("detallesSesionTutoria");
        }
        public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        {
            public DateOnlyConverter() : base(
                v => v.ToDateTime(TimeOnly.MinValue).Date,
                v => new DateOnly(v.Year, v.Month, v.Day))
            {
            }
        }

        public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
        {
            public TimeOnlyConverter() : base(
                v => v.ToTimeSpan(),
                v => TimeOnly.FromTimeSpan(v))
            {
            }
        }
    }
}
