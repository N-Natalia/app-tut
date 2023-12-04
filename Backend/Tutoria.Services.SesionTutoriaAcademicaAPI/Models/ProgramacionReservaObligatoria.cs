using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Enums;

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Models
{
    public class ProgramacionReservaObligatoria
    {
        [Key]
        public int IdProgramacion { get; set; }
        [Required]
        public string? IdTutor { get; set; }        
        public int Duracion { get; set; }
        public int TotalBloques { get; set; }
        public TipoReunion Tipo { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public bool Activo { get; set; }    

    }
    
}
