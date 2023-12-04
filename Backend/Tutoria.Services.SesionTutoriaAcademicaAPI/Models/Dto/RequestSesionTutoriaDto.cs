using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Dto
{
    public class RequestSesionTutoriaDto
    {
        public int IdSesionTutoria { get; set; }
        public int IdReserva { get; set; }
        public string? IdTutorado { get; set; }
        public string? IdTutor { get; set; }
        public int IdSemestre { get; set; }
        public DateOnly FechaReunion { get; set; }
        public TimeOnly Hora { get; set; }
    }
}
