using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Models
{
    public class SesionTutoria
    {
        [Key]
        public int IdSesionTutoria { get; set; }
        [Required]
        public int IdReserva { get; set; }
        [ForeignKey("IdReserva")]
        public virtual Reserva Reserva { get; set; }

        public string? IdTutorado { get; set; }
        public string? IdTutor { get; set; }
        public int IdSemestre { get; set; }

        public DateOnly FechaReunion { get; set; }
 
        public TimeOnly Hora { get; set; }

    }
}
