using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace Tutoria.Services.ConfiguracionAcademicaAPI.Models
{
    public class Person
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string? Code { get; set; }
        public string? Nombres { get; set; }     
        public string? ApPaterno { get; set; }   
        public string? ApMaterno { get; set; }
        public string? Email { get; set; }
    }
}
