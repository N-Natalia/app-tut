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
    public class ProgramacionReservaObligatoriaController : ControllerBase
    {
        private readonly AppDbContext _sesionTutoriaAcademicaContext;

        public ProgramacionReservaObligatoriaController(AppDbContext context)
        {
            _sesionTutoriaAcademicaContext = context;
        }

        //===============================================================================
        //          PROGRAMACION RESERVA OBLIGATORIA
        //===============================================================================
        // Agregar programacion reserva obligatoria
        //-------------------------------------------------------------------------------
        [HttpPost()]
        public async Task<IActionResult> AgregarProgramacionReservaObligatoria([FromBody] ProgramacionReservaObligatoria programacionReservaObligObj)
        {
            if (programacionReservaObligObj == null)
            {
                return BadRequest();
            }
            if (await CheckExistPrograReservaObligAsync(programacionReservaObligObj.IdTutor!,programacionReservaObligObj.FechaInicio))
            {
                return BadRequest(new { message = "Ya existe una programacion con esta fecha registrado por usted!" });
            }



            //Agregar
            await _sesionTutoriaAcademicaContext.ProgramacionesReservaObligatoria.AddAsync(programacionReservaObligObj);
            //Guardar
            await _sesionTutoriaAcademicaContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Programacion reserva obligatorio registrado!"
            });

        }
        private Task<bool> CheckExistPrograReservaObligAsync(string codeTutor, DateOnly fecha)
            => _sesionTutoriaAcademicaContext.ProgramacionesReservaObligatoria.AnyAsync(x => x.IdTutor == codeTutor && x.FechaInicio == fecha);
        

        //-------------------------------------------------------------------------------
        // Update programacion reserva obligatoria
        //-------------------------------------------------------------------------------
        [HttpPut()]
        public async Task<IActionResult> UpdateProgramacionReservaObligatoria([FromBody] ProgramacionReservaObligatoria programacionROActualizado)
        {
            if (programacionROActualizado == null)
                return BadRequest();
            //Recuperar programacion de reserva obligatoia existente por su code
            var programacionReservaObligExistente = await _sesionTutoriaAcademicaContext.ProgramacionesReservaObligatoria.SingleOrDefaultAsync(x => x.IdProgramacion == programacionROActualizado.IdProgramacion);
            if (programacionReservaObligExistente == null)
            {
                return NotFound();//La programcion de reserva obligatoria no fue encontrado
            }

            //actualizar los campos especificos de la programacion
            programacionReservaObligExistente.FechaInicio = programacionROActualizado.FechaInicio;
            programacionReservaObligExistente.FechaFin = programacionROActualizado.FechaFin;
            programacionReservaObligExistente.Duracion = programacionROActualizado.Duracion;
            programacionReservaObligExistente.Tipo = programacionROActualizado.Tipo;
            programacionReservaObligExistente.TotalBloques = programacionROActualizado.TotalBloques;

            //guardar los cambios en la base de datos
            try
            {
                await _sesionTutoriaAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Programacion de reserva obligatoria fue actualizado con exito!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar la programacion de reserva obligatoria.");
            }
        }

        //-------------------------------------------------------------------------------
        // Delete programacion reserva obligatoria by idProgramacion
        //-------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgramacionReservaObligatoria(int id)
        {
            var programacionReservaObligatoria = await _sesionTutoriaAcademicaContext.ProgramacionesReservaObligatoria.FirstAsync(x => x.IdProgramacion == id);

            if (programacionReservaObligatoria == null)
                return NotFound();

            //Eliminar objeto
            _sesionTutoriaAcademicaContext.ProgramacionesReservaObligatoria.Remove(programacionReservaObligatoria);
            //Guardar cambios
            await _sesionTutoriaAcademicaContext.SaveChangesAsync();


            return Ok(new
            {
                Message = "Programacion Reserva Obligatoria fue eliminado!"
            });
        }
        //-------------------------------------------------------------------------------
        // Get programaciones de reservas obligatorias lista
        //-------------------------------------------------------------------------------
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ResponseProgramacionReservaObligatoriaDto>>> GetProgramacionesReservasObligatorias()
        {
            var tipoReunionType = typeof(TipoReunion);
            return await _sesionTutoriaAcademicaContext.ProgramacionesReservaObligatoria
                            .Select(programacion => new ResponseProgramacionReservaObligatoriaDto
                            {

                                IdProgramacion = programacion.IdProgramacion,
                                IdTutor = programacion.IdTutor,
                                Duracion = programacion.Duracion,
                                TotalBloques = programacion.TotalBloques,
                                Tipo = Enum.GetName(tipoReunionType, programacion.Tipo),
                                FechaInicio = programacion.FechaInicio,
                                FechaFin = programacion.FechaFin,
                                Activo = programacion.Activo
                                
                            }).ToListAsync();
            
        }

        //-------------------------------------------------------------------------------
        // Get programaciones de reservas obligatorias lista Activos
        //-------------------------------------------------------------------------------
        [HttpGet("ListaActivos")]
        public async Task<ActionResult<IEnumerable<ResponseProgramacionReservaObligatoriaDto>>> GetProgramacionesReservasObligatoriaActivos()
        {
            var tipoReunionType = typeof(TipoReunion);
            return await _sesionTutoriaAcademicaContext.ProgramacionesReservaObligatoria
                            .Where(x => x.Activo == true)
                            .Select(programacion => new ResponseProgramacionReservaObligatoriaDto
                            {

                                IdProgramacion = programacion.IdProgramacion,
                                IdTutor = programacion.IdTutor,
                                Duracion = programacion.Duracion,
                                TotalBloques = programacion.TotalBloques,
                                Tipo = Enum.GetName(tipoReunionType, programacion.Tipo),
                                FechaInicio = programacion.FechaInicio,
                                FechaFin = programacion.FechaFin,
                                Activo = programacion.Activo

                            }).ToListAsync();

            
        }


        //-------------------------------------------------------------------------------
        // Get progracion de reserva obligatoria by IDprogramacion
        //-------------------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseProgramacionReservaObligatoriaDto>> GetProgramacionReservaObligatoriaById(int id)
        {
            var programacionReservaObligatoria = await _sesionTutoriaAcademicaContext.ProgramacionesReservaObligatoria.FirstOrDefaultAsync(x => x.IdProgramacion == id);

            if (programacionReservaObligatoria == null)
                return NotFound();

            // Obtener el valor real del enum tipo
            string tipoReal = Enum.GetName(typeof(TipoReunion), programacionReservaObligatoria.Tipo)!;

            

            return (new ResponseProgramacionReservaObligatoriaDto
            {
                IdProgramacion = programacionReservaObligatoria.IdProgramacion,
                IdTutor = programacionReservaObligatoria.IdTutor,
                Duracion = programacionReservaObligatoria.Duracion,
                TotalBloques = programacionReservaObligatoria.TotalBloques,
                Tipo = tipoReal,
                FechaInicio = programacionReservaObligatoria.FechaInicio,
                FechaFin = programacionReservaObligatoria.FechaFin,
                Activo = programacionReservaObligatoria.Activo
            });

        }
        //-------------------------------------------------------------------------------
        // Get progracion de reserva obligatoria Activo by IdTutor
        //-------------------------------------------------------------------------------
        [HttpGet("ProgramacionActivoByCodeTutor/{id}")]
        public async Task<ActionResult<ResponseProgramacionReservaObligatoriaDto>> GetProgramacionReservaObligatoriaByCodeTutor(string id)
        {
            var programacionReservaObligatoria = await _sesionTutoriaAcademicaContext.ProgramacionesReservaObligatoria.FirstOrDefaultAsync(x => x.IdTutor == id && x.Activo == true);

            if (programacionReservaObligatoria == null)
                return NotFound();

            // Obtener el valor real del enum tipo
            string tipoReal = Enum.GetName(typeof(TipoReunion), programacionReservaObligatoria.Tipo)!;

            return (new ResponseProgramacionReservaObligatoriaDto
            {
                IdProgramacion = programacionReservaObligatoria.IdProgramacion,
                IdTutor = programacionReservaObligatoria.IdTutor,
                Duracion = programacionReservaObligatoria.Duracion,
                TotalBloques = programacionReservaObligatoria.TotalBloques,
                Tipo = tipoReal,
                FechaInicio = programacionReservaObligatoria.FechaInicio,
                FechaFin = programacionReservaObligatoria.FechaFin,
                Activo = programacionReservaObligatoria.Activo
            });

        }

        
        //-------------------------------------------------------------------------------
        // Desacticvar programacion de reserva obligatoria by codeTutor
        //-------------------------------------------------------------------------------
        [HttpPut("DesactivarByCodeTutor/{id}")]
        public async Task<IActionResult> PutDesactivarProgramacionReservaObligatoriaActivoByCodeTutor(string id)
        {
            //Recuperar programacion de reserva obligatoia existente por code tutor
            var programacionReservaObligExistente = await _sesionTutoriaAcademicaContext.ProgramacionesReservaObligatoria.SingleOrDefaultAsync(x => x.IdTutor == id && x.Activo == true);
            if (programacionReservaObligExistente == null)
            {
                return NotFound();//La programcion de reserva obligatoria no fue encontrado
            }
            //actualizar los campos especificos de la programacion
            programacionReservaObligExistente.Activo = false;

            //guardar los cambios en la base de datos
            try
            {
                await _sesionTutoriaAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Programacion Reserva obligatoria fue desactivado con exito!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar la programacion de reserva obligatoria.");
            }

        }
    }
}
