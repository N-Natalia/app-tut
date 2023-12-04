namespace Tutoria.Services.ConfiguracionAcademicaAPI.Models.Dto
{
    public class RequestUpdateTutoradoDto
    {
        public int NroCelular { get; set; }
        public string? Direccion { get; set; }
        public string? PersonReferencia { get; set; }
        public int NroCelularPersonaReferencia { get; set; }
    }
}
