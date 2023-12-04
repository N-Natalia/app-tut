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
    public class SesionTutoriaController : ControllerBase
    {
        private readonly AppDbContext _sesionTutoriaAcademicaContext;
        public SesionTutoriaController(AppDbContext context)
        {
            _sesionTutoriaAcademicaContext = context;
        }
        //===============================================================================
        //          SESION TUTORIA
        //===============================================================================
        [HttpPost()]
        public async Task<IActionResult> AgregarSesionTutoria([FromBody] RequestSesionTutoriaDto sesionTutoriaObj)
        {
            if (sesionTutoriaObj == null)
            {
                return BadRequest();
            }
            if (!await CheckIdReservaExistAsync(sesionTutoriaObj.IdReserva))
            {
                return BadRequest(new { message = "No existe la reserva para esta sesion de tutoría" });
            }
            if (await CheckIdReservaExistIntoSesionesAsync(sesionTutoriaObj.IdReserva))
            {
                return BadRequest(new { message = "Ya existe una sesion de tutoria para dicha reserva." });
            }

            // Crear una instancia de sesionTutoria
            var sesionTutoria = new SesionTutoria
            {
                IdSesionTutoria = sesionTutoriaObj.IdSesionTutoria,
                IdReserva = sesionTutoriaObj.IdReserva,
                IdTutorado = sesionTutoriaObj.IdTutorado,
                IdTutor = sesionTutoriaObj.IdTutor,
                IdSemestre = sesionTutoriaObj.IdSemestre,
                FechaReunion = sesionTutoriaObj.FechaReunion,
                Hora = sesionTutoriaObj.Hora
            };


            //Agregar
            await _sesionTutoriaAcademicaContext.SesionesTutoria.AddAsync(sesionTutoria);
            //Guardar
            await _sesionTutoriaAcademicaContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Sesion tutoría registrado!"
            });

        }
        private Task<bool> CheckIdReservaExistAsync(int idReserva)
            => _sesionTutoriaAcademicaContext.Reservas.AnyAsync(x => x.IdReserva == idReserva);
        private Task<bool> CheckIdReservaExistIntoSesionesAsync(int idReserva)
            => _sesionTutoriaAcademicaContext.SesionesTutoria.AnyAsync(x => x.IdReserva == idReserva);

        //-------------------------------------------------------------------------------
        // Actualizar sesion de tutoria
        //-------------------------------------------------------------------------------
        [HttpPut()]
        public async Task<IActionResult> UpdateSesionTutoria([FromBody] RequestSesionTutoriaDto sesionTutoriactualizado)
        {
            if (sesionTutoriactualizado == null)
                return BadRequest();
            //Recuperar sesionTutoria existente por su code
            var sesionTutoriaExistente = await _sesionTutoriaAcademicaContext.SesionesTutoria.SingleOrDefaultAsync(x => x.IdSesionTutoria == sesionTutoriactualizado.IdSesionTutoria);
            if (sesionTutoriaExistente == null)
            {
                return NotFound();//La sesion tutoria no fue encontrado
            }

            // Crear una instancia de sesionTutoria
            var sesionTutoria = new SesionTutoria
            {
                IdSesionTutoria = sesionTutoriactualizado.IdSesionTutoria,
                IdReserva = sesionTutoriactualizado.IdReserva,
                IdTutorado = sesionTutoriactualizado.IdTutorado,
                IdTutor = sesionTutoriactualizado.IdTutor,
                IdSemestre = sesionTutoriactualizado.IdSemestre,
                FechaReunion = sesionTutoriactualizado.FechaReunion,
                Hora = sesionTutoriactualizado.Hora
            };



            //actualizar los campos especificos del tutorado
            sesionTutoriaExistente.FechaReunion = sesionTutoria.FechaReunion;
            sesionTutoriaExistente.Hora = sesionTutoria.Hora;

            //guardar los cambios en la base de datos
            try
            {
                await _sesionTutoriaAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Sesion tutoria fue actualizado con exito!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar la sesion tutoria");
            }
        }
        //-------------------------------------------------------------------------------
        // Get sesion tutoria by IdSesion
        //-------------------------------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseSesionTutoriaDto>> GetSesionTutoriaByIdSesion(int id)
        {
            var sesionTutoria = await _sesionTutoriaAcademicaContext.SesionesTutoria.FirstOrDefaultAsync(x => x.IdSesionTutoria == id);

            if (sesionTutoria == null)
                return NotFound();

            return (new ResponseSesionTutoriaDto
            {
                IdSesionTutoria = sesionTutoria.IdSesionTutoria,
                IdReserva = sesionTutoria.IdReserva,
                IdTutorado = sesionTutoria.IdTutorado,
                IdTutor = sesionTutoria.IdTutor,
                IdSemestre = sesionTutoria.IdSemestre,
                FechaReunion = sesionTutoria.FechaReunion,
                Hora = sesionTutoria.Hora
            });

        }
        //-------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------
        // Eliminar sesion de tutoria
        //-------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSesionTutoria(int id)
        {
            var sesionTutoria = await _sesionTutoriaAcademicaContext.SesionesTutoria.FirstAsync(x => x.IdSesionTutoria == id);

            if (sesionTutoria == null)
                return NotFound();

            //Eliminar objeto
            _sesionTutoriaAcademicaContext.SesionesTutoria.Remove(sesionTutoria);
            //Guardar cambios
            await _sesionTutoriaAcademicaContext.SaveChangesAsync();


            return Ok(new
            {
                Message = "Sesion tutoria fue eliminado!"
            });
        }
        //-------------------------------------------------------------------------------
        // Get sesiones tutoria lista
        //-------------------------------------------------------------------------------
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ResponseSesionTutoriaDto>>> GetSesionesTutoria()
        {
            return await _sesionTutoriaAcademicaContext.SesionesTutoria
                .Select( x => new ResponseSesionTutoriaDto
                {
                    IdSesionTutoria = x.IdSesionTutoria,
                    IdReserva = x.IdReserva,
                    IdTutorado = x.IdTutorado,
                    IdTutor = x.IdTutor,
                    IdSemestre = x.IdSemestre,
                    FechaReunion = x.FechaReunion,
                    Hora = x.Hora
                })
                .ToListAsync();
        }
        //-------------------------------------------------------------------------------
        // Get sesion tutoria by IdReservva
        //-------------------------------------------------------------------------------
        [HttpGet("SesionByIdReserva/{id}")]
        public async Task<ActionResult<ResponseSesionTutoriaDto>> GetSesionTutoriaByIdReserva(int id)
        {
            var sesionTutoria = await _sesionTutoriaAcademicaContext.SesionesTutoria.FirstOrDefaultAsync(x => x.IdReserva == id);

            if (sesionTutoria == null)
                return NotFound();

            return (new ResponseSesionTutoriaDto
            {
                IdSesionTutoria = sesionTutoria.IdSesionTutoria,
                IdReserva = sesionTutoria.IdReserva,
                IdTutorado = sesionTutoria.IdTutorado,
                IdTutor = sesionTutoria.IdTutor,
                IdSemestre = sesionTutoria.IdSemestre,
                FechaReunion = sesionTutoria.FechaReunion,
                Hora = sesionTutoria.Hora
            });

        }
        
    }
}
