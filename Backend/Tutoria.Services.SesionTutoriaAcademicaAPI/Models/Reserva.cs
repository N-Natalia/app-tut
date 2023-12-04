using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Enums;

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Models
{
    public class Reserva
    {
        [Key] 
        public int IdReserva { get; set; }
        [Required]
        public int IdCargaTutoria { get; set; }        
        public int? IdProgramacionReservaObligatoria { get; set; }
        [ForeignKey("IdProgramacionReservaObligatoria")]
        public virtual ProgramacionReservaObligatoria ProgramacionReservaObligatoria { get; set; }
        public int? IdTutorHorarioDisponible { get; set; }
        [ForeignKey("IdTutorHorarioDisponible")]
        public virtual TutorHorarioDisponible TutorHorarioDisponible { get; set; }        
        public DateOnly Fecha { get; set; }        
        public TimeOnly HoraTutoria { get; set; }   
        public TipoReunion TipoReunion { get; set; }
        public TipoReserva TipoReserva { get; set; }
        public EstadoConfirmacion EstadoConfirmacion { get; set; }
        public string? EnlaceReunion { get; set; }
        public string? LugarReunion { get; set; }
        public bool Activo { get; set; }
    }
}
