using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Enums;

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Models
{
    public class DetalleSesionTutoria
    {
        [Key]
        public int IdDetalleSesionTutoria { get; set; }
        [Required]
        public int IdSesionTutoria {get; set; }
        [ForeignKey("IdSesionTutoria")]
        public virtual SesionTutoria? SesionTutoria { get; set; }
        public Dimension Dimension { get; set; }
        public string? Descripcion { get; set; }
        public string? Referencia { get; set; }
        public string? Observaciones { get; set; }

    }
}
