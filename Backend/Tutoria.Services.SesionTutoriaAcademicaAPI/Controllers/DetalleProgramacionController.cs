using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Dto;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models.Enums;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Models;
using Tutoria.Services.SesionTutoriaAcademicaAPI.Context;

namespace Tutoria.Services.SesionTutoriaAcademicaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleProgramacionController : ControllerBase
    {
        private readonly AppDbContext _sesionTutoriaAcademicaContext;

        public DetalleProgramacionController(AppDbContext context)
        {
            _sesionTutoriaAcademicaContext = context;
        }

        //===============================================================================
        //         DETALLE PROGRAMACION RESERVA OBLIGATORIA
        //===============================================================================
        // Agregar detalle programacion reserva obligatoria
        //-------------------------------------------------------------------------------
        [HttpPost()]
        public async Task<IActionResult> AgregarProgramacionReservaObligatoria([FromBody] RequestDetalleProgramacionDto detalleProgramacionReservaObligObj)
        {
            if (detalleProgramacionReservaObligObj == null)
            {
                return BadRequest();
            }

            // Convertir a tipo programacion

            var programacion = new DetalleProgramacionReservaObligatoria
            {
                IdDetalleProgramacionReservaObligatoria = detalleProgramacionReservaObligObj.IdDetalleProgramacionReservaObligatoria,
                IdProgramacion = detalleProgramacionReservaObligObj.IdProgramacion,
                NroBloque = detalleProgramacionReservaObligObj.NroBloque,
                Fecha = detalleProgramacionReservaObligObj.Fecha,
                HoraInicioSesionTutoria = detalleProgramacionReservaObligObj.HoraInicioSesionTutoria,
                Activo = detalleProgramacionReservaObligObj.Activo

            };
            if (!await CheckExistIdPrograReservaObligAsync(detalleProgramacionReservaObligObj.IdProgramacion))
            {
                return BadRequest(new { message = "No existe programacion para crear su detalle" });
            }



            //Agregar
            await _sesionTutoriaAcademicaContext.DetalleProgramacionReservaObligatoria.AddAsync(programacion);
            //Guardar
            await _sesionTutoriaAcademicaContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Programacion reserva obligatorio registrado!"
            });

        }
        private Task<bool> CheckExistIdPrograReservaObligAsync(int idProgramacion)
            => _sesionTutoriaAcademicaContext.ProgramacionesReservaObligatoria.AnyAsync(x => x.IdProgramacion == idProgramacion);


        //-------------------------------------------------------------------------------
        // Update detalle programacion reserva obligatoria
        //-------------------------------------------------------------------------------
        [HttpPut()]
        public async Task<IActionResult> UpdateDetalleProgramacionReservaObligatoria([FromBody] RequestDetalleProgramacionDto detallePogramacionROActualizado)
        {
            if (detallePogramacionROActualizado == null)
                return BadRequest();
            //Recuperar detalle programacion de reserva obligatoia existente por su code
            var detalleProgramacionReservaObligExistente = await _sesionTutoriaAcademicaContext.DetalleProgramacionReservaObligatoria.SingleOrDefaultAsync(x => x.IdDetalleProgramacionReservaObligatoria == detallePogramacionROActualizado.IdDetalleProgramacionReservaObligatoria);
            if (detalleProgramacionReservaObligExistente == null)
            {
                return NotFound();//El datalle programcion de reserva obligatoria no fue encontrado
            }

            //actualizar los campos especificos del detalle de programacion
            detalleProgramacionReservaObligExistente.Fecha = detallePogramacionROActualizado.Fecha;
            detalleProgramacionReservaObligExistente.HoraInicioSesionTutoria = detallePogramacionROActualizado.HoraInicioSesionTutoria;
            detalleProgramacionReservaObligExistente.Activo = detallePogramacionROActualizado.Activo;

            //guardar los cambios en la base de datos
            try
            {
                await _sesionTutoriaAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Detalle de programacion de reserva obligatoria fue actualizado con exito!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar la programacion de reserva obligatoria.");
            }
        }

        //-------------------------------------------------------------------------------
        // Delete detalle programacion reserva obligatoria by idProgramacion
        //-------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleProgramacionReservaObligatoria(int id)
        {
            var detalleProgramacionReservaObligatoria = await _sesionTutoriaAcademicaContext.DetalleProgramacionReservaObligatoria.FirstAsync(x => x.IdDetalleProgramacionReservaObligatoria == id);

            if (detalleProgramacionReservaObligatoria == null)
                return NotFound();

            //Eliminar objeto
            _sesionTutoriaAcademicaContext.DetalleProgramacionReservaObligatoria.Remove(detalleProgramacionReservaObligatoria);
            //Guardar cambios
            await _sesionTutoriaAcademicaContext.SaveChangesAsync();


            return Ok(new
            {
                Message = "Detalle de programacion Reserva Obligatoria fue eliminado!"
            });
        }
        //-------------------------------------------------------------------------------
        // Get detalles programaciones de reservas obligatorias lista
        //-------------------------------------------------------------------------------
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ResponseDetalleProgramacionDto>>> GetDetalleProgramacionesReservasObligatorias()
        {
            var tipoReunionType = typeof(TipoReunion);
            return await _sesionTutoriaAcademicaContext.DetalleProgramacionReservaObligatoria
                            .Select(detalleProgramacion => new ResponseDetalleProgramacionDto
                            {

                                IdDetalleProgramacionReservaObligatoria = detalleProgramacion.IdDetalleProgramacionReservaObligatoria,
                                IdProgramacion = detalleProgramacion.IdProgramacion,
                                NroBloque = detalleProgramacion.NroBloque,
                                Fecha = detalleProgramacion.Fecha,
                                HoraInicioSesionTutoria = detalleProgramacion.HoraInicioSesionTutoria,
                                Activo = detalleProgramacion.Activo

                            }).ToListAsync();

        }

        
        //-------------------------------------------------------------------------------
        // Get detalle progracion de reserva obligatoria activa by IDprogramacion
        //-------------------------------------------------------------------------------
        [HttpGet("DetalleProgramacionByIdProgramacion/{id}")]
        public async Task<ActionResult<ResponseDetalleProgramacionDto>> GetDetalleProgramacionReservaObligatoriaByIdProgramacion(int id)
        {
            var detalleProgramacionReservaObligatoria = await _sesionTutoriaAcademicaContext.DetalleProgramacionReservaObligatoria.FirstOrDefaultAsync(x => x.IdProgramacion == id && x.Activo == true);

            if (detalleProgramacionReservaObligatoria == null)
                return NotFound();


            return (new ResponseDetalleProgramacionDto
            {
                IdDetalleProgramacionReservaObligatoria = detalleProgramacionReservaObligatoria.IdDetalleProgramacionReservaObligatoria,
                IdProgramacion = detalleProgramacionReservaObligatoria.IdProgramacion,
                NroBloque = detalleProgramacionReservaObligatoria.NroBloque,
                Fecha = detalleProgramacionReservaObligatoria.Fecha,
                HoraInicioSesionTutoria = detalleProgramacionReservaObligatoria.HoraInicioSesionTutoria,
                Activo = detalleProgramacionReservaObligatoria.Activo
            });

        }
        
        //-------------------------------------------------------------------------------
        // Desacticvar detalle programacion de reserva obligatoria activa by idProgramacion
        //-------------------------------------------------------------------------------
        [HttpPut("DesactivarDetalleProgramacionByIdProgramacion/{id}")]
        public async Task<IActionResult> PutDesactivarDetalleProgramacionActivoByIdProgramacion(int id)
        {
            //Recuperar programacion de reserva obligatoia existente por code tutor
            var detalleProgramacionReservaObligExistente = await _sesionTutoriaAcademicaContext.DetalleProgramacionReservaObligatoria.SingleOrDefaultAsync(x => x.IdProgramacion == id && x.Activo == true);
            if (detalleProgramacionReservaObligExistente == null)
            {
                return NotFound();//el detalle de programcion de reserva obligatoria no fue encontrado
            }
            //actualizar los campos especificos del detalle programacion
            detalleProgramacionReservaObligExistente.Activo = false;

            //guardar los cambios en la base de datos
            try
            {
                await _sesionTutoriaAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Detalle de programacion reserva obligatoria fue desactivado con exito!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar el detalle de programacion de reserva obligatoria.");
            }

        }
        
        //-------------------------------------------------------------------------------
        // Get detalle programacion by IdProgramacion
        //-------------------------------------------------------------------------------
        [HttpGet("DetallesProgramacionByIdProgramacion/{id}")]
        public async Task<ActionResult<IEnumerable<ResponseDetalleProgramacionDto>>> GetDetallesProgramacionByIdProgramacion(int id)
        {
            var dimensionType = typeof(Dimension);

            var detalles = await _sesionTutoriaAcademicaContext.DetalleProgramacionReservaObligatoria
                .Where(r => r.IdProgramacion == id)
                .Select(detalle => new ResponseDetalleProgramacionDto
                {
                    IdDetalleProgramacionReservaObligatoria = detalle.IdDetalleProgramacionReservaObligatoria,
                    IdProgramacion = detalle.IdProgramacion,
                    NroBloque = detalle.NroBloque,
                    Fecha = detalle.Fecha,
                    HoraInicioSesionTutoria = detalle.HoraInicioSesionTutoria,
                    Activo = detalle.Activo
                })
                .ToListAsync(); 
            return detalles;

        }
    }
}
