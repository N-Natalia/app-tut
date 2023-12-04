using System.ComponentModel.DataAnnotations.Schema;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Enums;

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Dto
{
    public class RequestReservaByIdHorarioHoraFechaDto
    {
        public int IdTutorHorarioDisponible { get; set; }
        public TimeOnly Hora { get; set; }
        public DateOnly Fecha { get; set; }
        
    }
}
