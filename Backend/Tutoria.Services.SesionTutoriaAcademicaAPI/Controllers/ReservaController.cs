using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Context;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Dto;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Enums;

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservaController : ControllerBase
    {
        private readonly AppDbContext _sesionTutoriaAcademicaContext;

        public ReservaController(AppDbContext context)
        {
            _sesionTutoriaAcademicaContext = context;
        }


        //===============================================================================
        //          RESERVA
        //===============================================================================
        //  Agregar reserva
        //-------------------------------------------------------------------------------
        [HttpPost()]
        public async Task<IActionResult> AgregarReserva([FromBody] RequestReservaDto reservaObj)
        {

            if (reservaObj == null)
            {
                return BadRequest();
            }

            // Convertir a tipo Reserva
            // Crear una instancia de CargaTutoria

            var reserva = new Reserva
            {
                IdReserva = reservaObj.IdReserva,
                IdCargaTutoria = reservaObj.IdCargaTutoria,
                IdProgramacionReservaObligatoria = reservaObj.IdProgramacionReservaObligatoria,
                IdTutorHorarioDisponible = reservaObj.IdTutorHorarioDisponible,
                Fecha = reservaObj.Fecha,
                HoraTutoria = reservaObj.HoraTutoria,
                TipoReunion = (TipoReunion)reservaObj.TipoReunion!,
                TipoReserva = (TipoReserva)reservaObj.TipoReserva!,
                EstadoConfirmacion = (EstadoConfirmacion)reservaObj.EstadoConfirmacion!,
                EnlaceReunion = reservaObj.EnlaceReunion,
                LugarReunion = reservaObj.LugarReunion,
                Activo = reservaObj.Activo
            };

            
            // Mismo tutorado realize reserva dos veces la misma fecha, hora
            if ( await CheckExistReserva_SameFechaHoraIdCargaAsync(reserva.Fecha,reserva.HoraTutoria, reserva.IdCargaTutoria))
                return BadRequest(new { message = "Ya se hizo una reserva en esta fecha y hora por usted!" });


            // Diferentews tutorados realizen reserva en la misma fecha, hora, con mismo tutor
            var tutorId = await GetTutorIdForReservaAsync(reserva);
            if (await CheckReservaExistAsync(reserva.Fecha, reserva.HoraTutoria, tutorId!))
                return BadRequest(new { message = "Ya se realizo una reserva en esta fecha y hora para el mismo tutor!!" });

            // Un tutorado solo realice una reserva en una fecha dada(el mismo dia)
            if (await CheckExistReserva_SameCarga_Fecha(reserva.Fecha,reserva.IdCargaTutoria))
                return BadRequest(new { message = "Solo se puede realizar una reserva por fecha(dia)!" });
                       

            //Agregar

            await _sesionTutoriaAcademicaContext.Reservas.AddAsync(reserva);

            //Guardar
            await _sesionTutoriaAcademicaContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Reserva registrado!"
            });

        }
       
        //private Task<bool> CheckExistReservaVoluntariaActivoAsync(int idCarga)
        //    => _sesionTutoriaAcademicaContext.Reservas.AnyAsync(x =>
        //        {
        //            Console.WriteLine("tipoReserva:", x.TipoReserva);
        //            return x.IdCargaTutoria == idCarga && x.Activo && x.TipoReserva == TipoReserva.Voluntario;
        //        }
        //    );

        private Task<bool> CheckExistReserva_SameFechaHoraIdCargaAsync(DateOnly fecha, TimeOnly hora, int idCarga)
            => _sesionTutoriaAcademicaContext.Reservas.AnyAsync(x => x.Fecha == fecha && x.HoraTutoria == hora && x.IdCargaTutoria == idCarga && x.TipoReserva == TipoReserva.Voluntario);

        private Task<bool> CheckExistReserva_SameCarga_Fecha(DateOnly fecha, int idCarga)
            => _sesionTutoriaAcademicaContext.Reservas.AnyAsync(x => x.Fecha == fecha && x.IdCargaTutoria == idCarga);
        private async Task<string?> GetTutorIdForReservaAsync(Reserva reservaObj)
        {
            if (reservaObj.IdProgramacionReservaObligatoria > 0)
            {
                return await _sesionTutoriaAcademicaContext.ProgramacionesReservaObligatoria
                    .Where(x => x.IdProgramacion == reservaObj.IdProgramacionReservaObligatoria)
                    .Select(x => x.IdTutor)
                    .FirstOrDefaultAsync();
            }
            else if (reservaObj.IdTutorHorarioDisponible > 0)
            {
                return await _sesionTutoriaAcademicaContext.TutorHorariosDisponible
                    .Where(x => x.IdHorario == reservaObj.IdTutorHorarioDisponible)
                    .Select(x => x.IdTutor)
                    .FirstOrDefaultAsync();
            }
            return null;
        }

        private async Task<bool> CheckReservaExistAsync(DateOnly fecha, TimeOnly hora, string IdTutor)
        {
            var reservasExistentes = await _sesionTutoriaAcademicaContext.Reservas
                                        .Where(x => x.Fecha == fecha && x.HoraTutoria == hora && x.IdTutorHorarioDisponible!=null)
                                        .ToListAsync();
            foreach (var reserva in reservasExistentes)
            {
                var tutorId = await GetTutorIdForReservaAsync(reserva);
                if (tutorId!.Equals(IdTutor))
                {
                    return true;
                }
            }

            return false;
        }

        //private async Task<bool> CheckReservaExistAsync(DateOnly fecha, TimeOnly hora, string IdTutor)
        //     => await _sesionTutoriaAcademicaContext.Reservas.AnyAsync(x => x.Fecha == fecha && x.HoraTutoria == hora && GetTutorIdForReservaAsync(x).Equals(IdTutor));

        //-------------------------------------------------------------------------------
        //  Get Reserva By IdTutorHorarioDisponible, hora , fecha
        //-------------------------------------------------------------------------------
        [HttpPost("ReservaByIdHorarioHoraFecha")]
        public async Task<ActionResult<ResponseReservaDto>> GetReservaByIdHorarioHoraFecha(
            [FromBody] RequestReservaByIdHorarioHoraFechaDto idHorarioHoraFechaObj)
        {
            var reserva = await _sesionTutoriaAcademicaContext.Reservas.FirstOrDefaultAsync(x => x.IdTutorHorarioDisponible == idHorarioHoraFechaObj.IdTutorHorarioDisponible && x.HoraTutoria == idHorarioHoraFechaObj.Hora && x.Fecha == idHorarioHoraFechaObj.Fecha);

            if (reserva == null)
                return NotFound();

            // Obtener el valor real del enum tipo reunion
            string tipoReunionReal = Enum.GetName(typeof(TipoReunion), reserva.TipoReunion)!;

            // Obtener el valor real del enum tipo reserva
            string tipoReservaReal = Enum.GetName(typeof(TipoReserva), reserva.TipoReserva)!;

            // Obtener el valor real del enum estado de confirmacion
            string estadoConfirmacionReal = Enum.GetName(typeof(EstadoConfirmacion), reserva.EstadoConfirmacion)!;

            return (new ResponseReservaDto
            {
                IdReserva = reserva.IdReserva,
                IdCargaTutoria = reserva.IdCargaTutoria,
                IdProgramacionReservaObligatoria = reserva.IdProgramacionReservaObligatoria,
                IdTutorHorarioDisponible = reserva.IdTutorHorarioDisponible,
                Fecha = reserva.Fecha,
                HoraTutoria = reserva.HoraTutoria,
                TipoReunion = tipoReunionReal,
                TipoReserva = tipoReservaReal,
                EstadoConfirmacion = estadoConfirmacionReal,
                EnlaceReunion = reserva.EnlaceReunion,
                LugarReunion = reserva.LugarReunion,
                Activo = reserva.Activo
            });

        }

        //-------------------------------------------------------------------------------
        //  Actualizar reserva
        //-------------------------------------------------------------------------------
        [HttpPut()]
        public async Task<IActionResult> UpdateReserva([FromBody] RequestReservaDto reservaActualizado)
        {
            if (reservaActualizado == null)
                return BadRequest();
            //Recuperar reservaActualizado existente por su code
            var reservaExistente = await _sesionTutoriaAcademicaContext.Reservas.SingleOrDefaultAsync(x => x.IdReserva == reservaActualizado.IdReserva);
            if (reservaExistente == null)
            {
                return NotFound();//La reserva no fue encontrado
            }

            // Crear una instancia de CargaTutoria
            var reserva = new Reserva
            {
                IdReserva = reservaActualizado.IdReserva,
                IdCargaTutoria = reservaActualizado.IdCargaTutoria,
                IdProgramacionReservaObligatoria = reservaActualizado.IdProgramacionReservaObligatoria,
                IdTutorHorarioDisponible = reservaActualizado.IdTutorHorarioDisponible,
                Fecha = reservaActualizado.Fecha,
                HoraTutoria = reservaActualizado.HoraTutoria,
                TipoReunion = (TipoReunion)reservaActualizado.TipoReunion!,
                TipoReserva = (TipoReserva)reservaActualizado.TipoReserva!,
                EstadoConfirmacion = (EstadoConfirmacion)reservaActualizado.EstadoConfirmacion!,
                EnlaceReunion = reservaActualizado.EnlaceReunion,
                LugarReunion = reservaActualizado.LugarReunion,
                Activo = reservaActualizado.Activo
            };


            //actualizar los campos especificos de la reserva
            reservaExistente.Fecha = reserva.Fecha;
            reservaExistente.HoraTutoria = reserva.HoraTutoria;
            reservaExistente.TipoReunion = reserva.TipoReunion;
            reservaExistente.EnlaceReunion = reserva.EnlaceReunion;
            reservaExistente.LugarReunion = reserva.LugarReunion;

            //guardar los cambios en la base de datos
            try
            {
                await _sesionTutoriaAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Reserva fue actualizado con exito!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar la reserva");
            }
        }


        //-------------------------------------------------------------------------------
        //  Actualizar reserva
        //-------------------------------------------------------------------------------
        [HttpPut("DesactivarReservaVoluntarioByIdCarga/{id}")]
        public async Task<IActionResult> DesactivarReservaVoluntaria(int id)
        {
            
            //Recuperar reservaActualizado existente por su code
            var reservaExistente = await _sesionTutoriaAcademicaContext.Reservas.SingleOrDefaultAsync(x => x.IdCargaTutoria == id && x.Activo == true && x.TipoReserva == 0);
            if (reservaExistente == null)
            {
                return NotFound();//La reserva no fue encontrado
            }

            //actualizar los campos especificos de la reserva
            reservaExistente.Activo = false;

            //guardar los cambios en la base de datos
            try
            {
                await _sesionTutoriaAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Reserva fue desactivado exitosamente!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al desactivar la reserva");
            }
        }


        //-------------------------------------------------------------------------------
        //  Eliminar reserva
        //-------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _sesionTutoriaAcademicaContext.Reservas.FirstAsync(x => x.IdReserva == id);

            if (reserva == null)
                return NotFound();

            //Eliminar objeto
            _sesionTutoriaAcademicaContext.Reservas.Remove(reserva);
            //Guardar cambios
            await _sesionTutoriaAcademicaContext.SaveChangesAsync();


            return Ok(new
            {
                Message = "Reserva fue eliminado!"
            });
        }


        //-------------------------------------------------------------------------------
        //  Lista de reservas
        //-------------------------------------------------------------------------------

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ResponseReservaDto>>> GetReservas()
        {
            var tipoReunionType = typeof(TipoReunion);
            var tipoReservaType = typeof(TipoReserva);
            var estadoConfirmacionType = typeof(EstadoConfirmacion);

            return await _sesionTutoriaAcademicaContext.Reservas
                .Select(reserva => new ResponseReservaDto
                {
                    IdReserva = reserva.IdReserva,
                    IdCargaTutoria = reserva.IdCargaTutoria,
                    IdProgramacionReservaObligatoria = reserva.IdProgramacionReservaObligatoria,
                    IdTutorHorarioDisponible = reserva.IdTutorHorarioDisponible,
                    Fecha = reserva.Fecha,
                    HoraTutoria = reserva.HoraTutoria,
                    TipoReunion = Enum.GetName(tipoReunionType, reserva.TipoReunion),
                    TipoReserva = Enum.GetName(tipoReservaType, reserva.TipoReserva),
                    EstadoConfirmacion = Enum.GetName(estadoConfirmacionType, reserva.EstadoConfirmacion),
                    EnlaceReunion = reserva.EnlaceReunion,
                    LugarReunion = reserva.LugarReunion,
                    Activo = reserva.Activo
                })
                .ToListAsync();
        }

        


        //-------------------------------------------------------------------------------
        //  Get Reserva By Id Reserva
        //-------------------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseReservaDto>> GetReservaById(int id)
        {
            var reserva = await _sesionTutoriaAcademicaContext.Reservas.FirstOrDefaultAsync(x => x.IdReserva == id);

            if (reserva == null)
                return NotFound();

            // Obtener el valor real del enum tipo reunion
            string tipoReunionReal = Enum.GetName(typeof(TipoReunion), reserva.TipoReunion)!;

            // Obtener el valor real del enum tipo reserva
            string tipoReservaReal = Enum.GetName(typeof(TipoReserva), reserva.TipoReserva)!;

            // Obtener el valor real del enum estado de confirmacion
            string estadoConfirmacionReal = Enum.GetName(typeof(EstadoConfirmacion), reserva.EstadoConfirmacion)!;

            return (new ResponseReservaDto
            {
                IdReserva = reserva.IdReserva,
                IdCargaTutoria = reserva.IdCargaTutoria,
                IdProgramacionReservaObligatoria = reserva.IdProgramacionReservaObligatoria,
                IdTutorHorarioDisponible = reserva.IdTutorHorarioDisponible,
                Fecha = reserva.Fecha,
                HoraTutoria = reserva.HoraTutoria,
                TipoReunion = tipoReunionReal,
                TipoReserva = tipoReservaReal,
                EstadoConfirmacion = estadoConfirmacionReal,
                EnlaceReunion = reserva.EnlaceReunion,
                LugarReunion = reserva.LugarReunion,
                Activo = reserva.Activo,
            });

        }
        
        //-------------------------------------------------------------------------------
        //  Get Reserva Voluntario activo
        //-------------------------------------------------------------------------------
        [HttpGet("reservaVoluntarioActivoByIdCarga/{id}")]
        public async Task<ActionResult<ResponseReservaDto>> GetReservaVoluntarioActivo(int id)
        {
            var reserva = await _sesionTutoriaAcademicaContext.Reservas.FirstOrDefaultAsync(x => x.IdCargaTutoria == id && x.Activo == true && x.TipoReserva == 0);

            if (reserva == null)
                return NotFound();

            // Obtener el valor real del enum tipo reunion
            string tipoReunionReal = Enum.GetName(typeof(TipoReunion), reserva.TipoReunion)!;

            // Obtener el valor real del enum tipo reserva
            string tipoReservaReal = Enum.GetName(typeof(TipoReserva), reserva.TipoReserva)!;

            // Obtener el valor real del enum estado de confirmacion
            string estadoConfirmacionReal = Enum.GetName(typeof(EstadoConfirmacion), reserva.EstadoConfirmacion)!;

            return (new ResponseReservaDto
            {
                IdReserva = reserva.IdReserva,
                IdCargaTutoria = reserva.IdCargaTutoria,
                IdProgramacionReservaObligatoria = reserva.IdProgramacionReservaObligatoria,
                IdTutorHorarioDisponible = reserva.IdTutorHorarioDisponible,
                Fecha = reserva.Fecha,
                HoraTutoria = reserva.HoraTutoria,
                TipoReunion = tipoReunionReal,
                TipoReserva = tipoReservaReal,
                EstadoConfirmacion = estadoConfirmacionReal,
                EnlaceReunion = reserva.EnlaceReunion,
                LugarReunion = reserva.LugarReunion,
                Activo = reserva.Activo,
            });

        }
        //-------------------------------------------------------------------------------
        //  Get Reserva por intervalo de fecha x Tipo Reserva Voluntario
        //-------------------------------------------------------------------------------
        [HttpGet("ReservasPorIntervaloFecha-TipoVoluntario")]
        public async Task<ActionResult<IEnumerable<ResponseReservaDto>>> GetReservasPorFechaYTipoVoluntario(
            [FromQuery(Name = "anioInicio")] int anioInicio,
            [FromQuery(Name = "mesInicio")] int mesInicio,
            [FromQuery(Name = "diaInicio")] int diaInicio,
            [FromQuery(Name = "anioLimite")] int anioLimite,
            [FromQuery(Name = "mesLimite")] int mesLimite,
            [FromQuery(Name = "diaLimite")] int diaLimite
            )
        {
            // Construye las fechas de inicio y fin a partir de los parámetros
            DateOnly fechaInicio = new DateOnly(anioInicio, mesInicio, diaInicio);
            DateOnly fechaFin = new DateOnly(anioLimite, mesLimite, diaLimite);

            Console.WriteLine( fechaInicio.ToString() );
            Console.WriteLine(fechaFin.ToString());


            var tipoReunionType = typeof(TipoReunion);
            var tipoReservaType = typeof(TipoReserva);
            var estadoConfirmacionType = typeof(EstadoConfirmacion);

            var reservas = await _sesionTutoriaAcademicaContext.Reservas
                .Where(r => r.TipoReserva == TipoReserva.Voluntario && r.Activo == true && r.Fecha >= fechaInicio && r.Fecha <= fechaFin)
                .Select(reserva => new ResponseReservaDto
                {
                    IdReserva = reserva.IdReserva,
                    IdCargaTutoria = reserva.IdCargaTutoria,
                    IdProgramacionReservaObligatoria = reserva.IdProgramacionReservaObligatoria,
                    IdTutorHorarioDisponible = reserva.IdTutorHorarioDisponible,
                    Fecha = reserva.Fecha,
                    HoraTutoria = reserva.HoraTutoria,
                    TipoReunion = Enum.GetName(tipoReunionType, reserva.TipoReunion),
                    TipoReserva = Enum.GetName(tipoReservaType, reserva.TipoReserva),
                    EstadoConfirmacion = Enum.GetName(estadoConfirmacionType, reserva.EstadoConfirmacion),
                    EnlaceReunion = reserva.EnlaceReunion,
                    LugarReunion = reserva.LugarReunion,
                    Activo = reserva.Activo

                })
                .ToListAsync();
            // Imprime las fechas de las reservas
            reservas.ForEach(reserva => Console.WriteLine(reserva.Fecha));
            return reservas;
        }



        //-------------------------------------------------------------------------------
        //  Get Reservas by idProgramacion Reserva
        //-------------------------------------------------------------------------------
        [HttpGet("ReservasByIdProgramacion/{id}")]
        public async Task<ActionResult<IEnumerable<ResponseReservaDto>>> GetReservasByIdProgramReservaOblig( int id)
        {
            var tipoReunionType = typeof(TipoReunion);
            var tipoReservaType = typeof(TipoReserva);
            var estadoConfirmacionType = typeof(EstadoConfirmacion);

            var reservas = await _sesionTutoriaAcademicaContext.Reservas
                .Where(r => r.IdProgramacionReservaObligatoria == id)
                .Select(reserva => new ResponseReservaDto
                {
                    IdReserva = reserva.IdReserva,
                    IdCargaTutoria = reserva.IdCargaTutoria,
                    IdProgramacionReservaObligatoria = reserva.IdProgramacionReservaObligatoria,
                    IdTutorHorarioDisponible = reserva.IdTutorHorarioDisponible,
                    Fecha = reserva.Fecha,
                    HoraTutoria = reserva.HoraTutoria,
                    TipoReunion = Enum.GetName(tipoReunionType, reserva.TipoReunion),
                    TipoReserva = Enum.GetName(tipoReservaType, reserva.TipoReserva),
                    EstadoConfirmacion = Enum.GetName(estadoConfirmacionType, reserva.EstadoConfirmacion),
                    EnlaceReunion = reserva.EnlaceReunion,
                    LugarReunion = reserva.LugarReunion,
                    Activo = reserva.Activo

                })
                .ToListAsync();

            return reservas;
        }
        
        //-------------------------------------------------------------------------------
        //  Actualilizar estado de confirmacion
        //-------------------------------------------------------------------------------
        [HttpPut("ConfirmarByIdReserva/{id}")]
        public async Task<IActionResult> PutEstadoConfirmacionByIdReserva(int id)
        {
            //Recuperar programacion de reserva obligatoia existente por code tutor
            var reservaExistente = await _sesionTutoriaAcademicaContext.Reservas.SingleOrDefaultAsync(x => x.IdReserva == id);
            if (reservaExistente == null)
            {
                return NotFound();//La programcion de reserva obligatoria no fue encontrado
            }
            //actualizar los campos especificos de la programacion
            reservaExistente.EstadoConfirmacion = EstadoConfirmacion.Confirmado;

            //guardar los cambios en la base de datos
            try
            {
                await _sesionTutoriaAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "La reserva fue confirmado con exito!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar la reserva.");
            }

        }

        //-------------------------------------------------------------------------------
        //  Actualilizar estado de confirmacion
        //-------------------------------------------------------------------------------
        [HttpPut("CancelarConfirmacionByIdReserva/{id}")]
        public async Task<IActionResult> PutCancelarConfirmacionByIdReserva(int id)
        {
            //Recuperar reserva by id
            var reservaExistente = await _sesionTutoriaAcademicaContext.Reservas.SingleOrDefaultAsync(x => x.IdReserva == id);
            if (reservaExistente == null)
            {
                return NotFound();//La programcion de reserva obligatoria no fue encontrado
            }
            //actualizar los campos especificos de la reserva
            reservaExistente.EstadoConfirmacion = EstadoConfirmacion.Solicitado;

            //guardar los cambios en la base de datos
            try
            {
                await _sesionTutoriaAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "La confirmacion de reserva fue cancelada con exito!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al cancelar confirmacion de reserva.");
            }

        }
    }
}
