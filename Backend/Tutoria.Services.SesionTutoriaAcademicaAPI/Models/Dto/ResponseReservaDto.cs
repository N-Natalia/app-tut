namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Dto
{
    public class ResponseReservaDto
    {
        public int IdReserva { get; set; }
        public int IdCargaTutoria { get; set; }
        public int? IdProgramacionReservaObligatoria { get; set; }
        public int? IdTutorHorarioDisponible { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly HoraTutoria { get; set; }
        public string? TipoReunion { get; set; }
        public string? TipoReserva { get; set; }
        public string? EstadoConfirmacion { get; set; }
        public string? EnlaceReunion { get; set; }
        public string? LugarReunion { get; set; }
        public bool Activo { get; set; }
    }
}
