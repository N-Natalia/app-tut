using System.ComponentModel.DataAnnotations;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Enums;

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Dto
{
    public class ResponseProgramacionReservaObligatoriaDto
    {
        public int IdProgramacion { get; set; }
        public string? IdTutor { get; set; }
        public int Duracion { get; set; }
        public int TotalBloques { get; set; }
        public string? Tipo { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public bool Activo { get; set; }
    }
}
