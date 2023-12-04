using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Enums;

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Dto
{
    public class RequestDetalleTutoriaDto
    {
        public int IdDetalleSesionTutoria { get; set; }
        public int IdSesionTutoria { get; set; }
        public Dimension Dimension { get; set; }
        public string? Descripcion { get; set; }
        public string? Referencia { get; set; }
        public string? Observaciones { get; set; }
    }
}
