using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Enums;

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Models
{
    public class TutorHorarioDisponible
    {
        [Key]
        public int IdHorario { get; set; }
        [Required]
        public string? IdTutor { get; set;}

        public TimeOnly Hora { get; set;}
        public int Duracion { get; set;}
        public string? Dia { get; set;}
        public TipoReunion Tipo { get; set;}
        public bool Activo { get; set;}
        
    }

}
