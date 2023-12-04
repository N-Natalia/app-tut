using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tutoria.Services.ConfiguracionAcademicaAPI.Models
{
    public class Tutor: Person
    {
        public int NroCelular { get; set; }
        public string? LugarReunion { get; set; }
        public string? EnlaceReunion { get; set; }

    }
}
