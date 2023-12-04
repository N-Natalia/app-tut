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
    public class DetalleTutoriaController : ControllerBase
    {
        private readonly AppDbContext _sesionTutoriaAcademicaContext;
        public DetalleTutoriaController(AppDbContext context)
        {
            _sesionTutoriaAcademicaContext = context;
        }
        //===============================================================================
        //          DETALLE SESION TUTORIA
        //===============================================================================
        [HttpPost()]
        public async Task<IActionResult> AgregarDetalleTutoria([FromBody] RequestDetalleTutoriaDto detalleTutoriaObj)
        {
            if (detalleTutoriaObj == null)
            {
                return BadRequest();
            }
            if (!await CheckIdSesionExistAsync(detalleTutoriaObj.IdSesionTutoria))
            {
                return BadRequest(new { message = "No existe la sesion de tutoria para generar su detalle." });
            }
            if (await CheckDetalleToSesionExistAsync(detalleTutoriaObj.IdSesionTutoria))
            {
                return BadRequest(new { message = "El detalle ya fue generado para la sesion de tutoria." });
            }
            // Crear una instancia de detalle tutoria
            var detalleTutoria = new DetalleSesionTutoria
            {
                IdDetalleSesionTutoria = detalleTutoriaObj.IdDetalleSesionTutoria,
                IdSesionTutoria = detalleTutoriaObj.IdSesionTutoria,
                Dimension = detalleTutoriaObj.Dimension,
                Descripcion = detalleTutoriaObj.Descripcion,
                Referencia = detalleTutoriaObj.Referencia,
                Observaciones = detalleTutoriaObj.Observaciones
            };

            //Agregar
            await _sesionTutoriaAcademicaContext.DetalleSesionesTutoria.AddAsync(detalleTutoria);
            //Guardar
            await _sesionTutoriaAcademicaContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Detalle tutoría registrado!"
            });

        }
        private Task<bool> CheckIdSesionExistAsync(int idSesion)
            => _sesionTutoriaAcademicaContext.SesionesTutoria.AnyAsync(x => x.IdSesionTutoria == idSesion);
        private Task<bool> CheckDetalleToSesionExistAsync(int idSesion)
            => _sesionTutoriaAcademicaContext.DetalleSesionesTutoria.AnyAsync(x => x.IdSesionTutoria == idSesion);
        //-------------------------------------------------------------------------------//-------------------------------------------------------------------------------
        // Actualizar detalle de tutoria
        //-------------------------------------------------------------------------------
        [HttpPut()]
        public async Task<IActionResult> UpdateDetalleTutoria([FromBody] RequestDetalleTutoriaDto detalleTutoriactualizado)
        {
            if (detalleTutoriactualizado == null)
                return BadRequest();
            //Recuperar detalle tutoria existente por su code
            var detalleTutoriaExistente = await _sesionTutoriaAcademicaContext.DetalleSesionesTutoria.SingleOrDefaultAsync(x => x.IdDetalleSesionTutoria == detalleTutoriactualizado.IdDetalleSesionTutoria);
            if (detalleTutoriaExistente == null)
            {
                return NotFound();//La sesion tutoria no fue encontrado
            }


            // Crear una instancia de detalle Sesion Tutoria
            var detalleSesionTutoria = new DetalleSesionTutoria
            {
                IdDetalleSesionTutoria = detalleTutoriactualizado.IdDetalleSesionTutoria,
                IdSesionTutoria = detalleTutoriactualizado.IdSesionTutoria,
                Dimension = detalleTutoriactualizado.Dimension,
                Descripcion = detalleTutoriactualizado.Descripcion,
                Referencia = detalleTutoriactualizado.Referencia,
                Observaciones = detalleTutoriactualizado.Observaciones
            };


            //actualizar los campos especificos del tutorado
            detalleTutoriaExistente.Descripcion = detalleSesionTutoria.Descripcion;
            detalleTutoriaExistente.Referencia = detalleSesionTutoria.Referencia;
            detalleTutoriaExistente.Observaciones = detalleSesionTutoria.Observaciones;

            //guardar los cambios en la base de datos
            try
            {
                await _sesionTutoriaAcademicaContext.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Detalle Sesion tutoria fue actualizado con exito!"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Error al actualizar la sesion tutoria");
            }
        }
        //-------------------------------------------------------------------------------
        // Eliminar detalle de tutoria
        //-------------------------------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleTutoria(int id)
        {
            var detalleTutoria = await _sesionTutoriaAcademicaContext.DetalleSesionesTutoria.FirstAsync(x => x.IdDetalleSesionTutoria == id);

            if (detalleTutoria == null)
                return NotFound();

            //Eliminar objeto
            _sesionTutoriaAcademicaContext.DetalleSesionesTutoria.Remove(detalleTutoria);
            //Guardar cambios
            await _sesionTutoriaAcademicaContext.SaveChangesAsync();


            return Ok(new
            {
                Message = "Detalle tutoria fue eliminado!"
            });
        }
        //-------------------------------------------------------------------------------
        // Listar detalles tutoria
        //-------------------------------------------------------------------------------
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<ResponseDetalleTutoriaDto>>> GetDetallesTutoria()
        {
            var dimensionType = typeof(Dimension);

            var detalles = await _sesionTutoriaAcademicaContext.DetalleSesionesTutoria
                .Select(detalle => new ResponseDetalleTutoriaDto
                {
                    IdDetalleSesionTutoria = detalle.IdDetalleSesionTutoria,
                    IdSesionTutoria = detalle.IdSesionTutoria,
                    Dimension = Enum.GetName(dimensionType, detalle.Dimension),
                    Descripcion = detalle.Descripcion,
                    Referencia = detalle.Referencia,
                    Observaciones = detalle.Observaciones

                })
                .ToListAsync();

            return detalles;
        }
        //-------------------------------------------------------------------------------
        // Get detalle tutoria by IdSesionTutoria
        //-------------------------------------------------------------------------------
        [HttpGet("DetallesByIdSesion/{id}")]
        public async Task<ActionResult<IEnumerable<ResponseDetalleTutoriaDto>>> GetDetallesTutoriaByIdSesionTutoria(int id)
        {
            var dimensionType = typeof(Dimension);

            var detalles = await _sesionTutoriaAcademicaContext.DetalleSesionesTutoria
                .Where(r => r.IdSesionTutoria == id)
                .Select(detalle => new ResponseDetalleTutoriaDto
                {
                    IdDetalleSesionTutoria = detalle.IdDetalleSesionTutoria,
                    IdSesionTutoria = detalle.IdSesionTutoria,
                    Dimension = Enum.GetName(dimensionType, detalle.Dimension),
                    Descripcion = detalle.Descripcion,
                    Referencia = detalle.Referencia,
                    Observaciones = detalle.Observaciones
                })
                .ToListAsync(); 
            return detalles;

        }

        //-------------------------------------------------------------------------------
    }
}
