namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Dto
{
    public class ResponseDetalleProgramacionDto
    {
        public int IdDetalleProgramacionReservaObligatoria { get; set; }
        public int IdProgramacion { get; set; }
        public int NroBloque { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly HoraInicioSesionTutoria { get; set; }
        public bool Activo { get; set; }
    }
}
