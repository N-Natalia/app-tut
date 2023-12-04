using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using Tutoria.Services.ConfiguracionAcademicaAPI.Models;

namespace Tutoria.Services.ConfiguracionAcademicaAPI.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Tutor> Tutores { get; set; }
        public DbSet<Tutorado> Tutorados { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Semestre> Semestres { get; set; }
        public DbSet<CargaTutoria> CargasTutoria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().ToTable("persons");
            modelBuilder.Entity<Tutor>().ToTable("tutores");
            modelBuilder.Entity<Tutorado>().ToTable("tutorados");
            modelBuilder.Entity<Administrador>().ToTable("administradores");
            modelBuilder.Entity<Semestre>().ToTable("semestres");
            modelBuilder.Entity<CargaTutoria>().ToTable("cargasTutoria");
        }
    }
}
