using Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Enums;

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Dto
{
    public class RequestReservaDto
    {
        public int IdReserva { get; set; }
        public int IdCargaTutoria { get; set; }
        public int? IdProgramacionReservaObligatoria { get; set; }
        public int? IdTutorHorarioDisponible { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly HoraTutoria { get; set; }
        public TipoReunion? TipoReunion { get; set; }
        public TipoReserva? TipoReserva { get; set; }
        public EstadoConfirmacion? EstadoConfirmacion { get; set; }
        public string? EnlaceReunion { get; set; }
        public string? LugarReunion { get; set; }
        public bool Activo { get; set; }
    }
}
