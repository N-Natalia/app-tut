using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutoria.Services.ConfiguracionAcademicaAPI.Models
{
    public class CargaTutoria
    {
        [Key]
        public int IdCargaTutoria { get; set; }

        public string? IdTutor { get; set; }
        [ForeignKey("IdTutor")]        
        public Tutor? Tutor { get; set; }

        public string? IdTutorado { get; set; }
        [ForeignKey("IdTutorado")]        
        public Tutorado? Tutorado { get; set; }

        public int IdSemestre { get; set; }
        [ForeignKey("IdSemestre")]
        public Semestre? Semestre { get; set; }
        
        public bool Estado { get; set; }    

    }

}
