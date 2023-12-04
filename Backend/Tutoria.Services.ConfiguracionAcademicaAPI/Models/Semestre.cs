using System.ComponentModel.DataAnnotations;

namespace Tutoria.Services.ConfiguracionAcademicaAPI.Models
{
    public class Semestre
    {
        [Key]
        public int IdSemestre { get; set; }
        [Required]
        public string? DenominacionSemestre { get; set; }
        public bool Activo { get; set; }    
    }
}
