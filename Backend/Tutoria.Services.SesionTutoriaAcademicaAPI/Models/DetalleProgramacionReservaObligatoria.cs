using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Models
{
    public class DetalleProgramacionReservaObligatoria
    {
        [Key]
        public int IdDetalleProgramacionReservaObligatoria { get; set; }
       
        [Required]
        public int IdProgramacion { get; set; }
        [ForeignKey("IdProgramacion")]
        public virtual ProgramacionReservaObligatoria? ProgramacionReservaObligatoria { get; set; }

        public int NroBloque { get; set; }

        public DateOnly Fecha { get; set; }
        public TimeOnly HoraInicioSesionTutoria { get; set; }      

        public bool Activo { get; set; }    

    }
    
}
