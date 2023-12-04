namespace Tutoria.Services.ConfiguracionAcademicaAPI.Models.Dto
{
    public class RequestCargaDto
    {
        public string? IdTutor { get; set; }
        public string? IdTutorado { get; set; }
        public int IdSemestre { get; set; }
        public bool Estado { get; set; }
    }
}
