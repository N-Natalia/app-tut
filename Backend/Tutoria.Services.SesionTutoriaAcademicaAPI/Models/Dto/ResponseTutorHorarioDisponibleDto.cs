using System.ComponentModel.DataAnnotations;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Enums;

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Dto
{
    public class ResponseTutorHorarioDisponibleDto
    {
        public int IdHorario { get; set; }
        public string? IdTutor { get; set; }
        public TimeOnly Hora { get; set; }
        public int Duracion { get; set; }
        public string? Dia { get; set; }
        public string? Tipo { get; set; }
        public bool Activo { get; set; }
    }
}
